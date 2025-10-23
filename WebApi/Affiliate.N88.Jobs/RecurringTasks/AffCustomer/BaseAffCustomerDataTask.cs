using Affiliate.N88.Jobs.Apis;
using Affiliate.N88.Jobs.Configurations;
using System.Collections.Generic;
using System;
using Affiliate.N88.Repository;
using System.Threading.Tasks;
using Affiliate.N88.DTO;
using Affiliate.N88.Core;
using Affiliate.N88.Utitlities;
using System.Linq;
using Fanex.Athena.CronJobs.App.Helpers;
using Affiliate.N88.Jobs.Constants;
using Affiliate.N88.SpParams.Commands;
using Affiliate.N88.Exceptions;
using Affiliate.N88.Models;
using System.Text;
using System.Web;
using Fanex.Logging;

namespace Affiliate.N88.Jobs.RecurringTasks.AffCustomer
{
    public class BaseAffCustomerDataTask
    {
        protected virtual bool IsNeedRealData => false;

        protected readonly IAffMemberRepository _affMemberRepository;
        protected readonly ILicenseeApi _licenseeApi;
        protected readonly ILogger _logger;
        protected readonly FetchServiceSetting _fetchServiceSetting;
        protected readonly LicenseeSiteSetting _licenseeSiteSetting;

        protected IEnumerable<string> SupportedSites => _licenseeSiteSetting?.SupportedSites;

        public BaseAffCustomerDataTask(ILicenseeApi licenseeApi
            , IAffMemberRepository affMemberRepository
            , ILogger logger
            , LicenseeSiteSetting licenseeSiteSetting
            , FetchServiceSetting fetchServiceSetting)
        {
            _affMemberRepository = affMemberRepository;
            _licenseeApi = licenseeApi;
            _logger = logger;
            _licenseeSiteSetting = licenseeSiteSetting ?? throw new ArgumentNullException(nameof(licenseeSiteSetting));
            _fetchServiceSetting = fetchServiceSetting ?? throw new ArgumentNullException(nameof(fetchServiceSetting));
        }

        public async Task FetchDataAsync()
        {
            if (SupportedSites == null
                || !SupportedSites.Any())
            {
                return;
            }

            var maxTimeRange = GetApiMaxTimeRange();

            await FetchDataAsync(maxTimeRange);
        }

        public async Task FetchDataAsync(TimeSpan maxTimeRange)
        {
            var date = await GetMinLastedTimeAsync();
            var fromDate = GetAdjustmentTime(date);

            var apiServerNow = DateTimeHelper.GetApiServerDateTime();
            var dateRanges = DateTimeHelper.GetProportionTimeRange(fromDate, apiServerNow, maxTimeRange);

            await HandleFetchDataAsync(dateRanges);

            if (IsNeedRealData)
            {
                await HandleFetchRealDataAsync(dateRanges, apiServerNow);
            }
        }

        private async Task<DateTime> GetMinLastedTimeAsync()
        {
            var dates = new List<DateTime>();
            foreach (var site in SupportedSites)
            {
                try
                {
                    var siteCode = _licenseeSiteSetting.GetCodePrefix(site);
                    var siteNameInStore = await GetSiteNameAsync(siteCode);
                    if (string.Compare(site, siteNameInStore, StringComparison.OrdinalIgnoreCase) != 0)
                    {
                        await _logger.WarnAsync($"Site name in web config {site} isn't having same site name in database {siteNameInStore}");
                    }

                    var lastedDate = await GetLastedImportDate(siteCode, _fetchServiceSetting.DataSource);
                    dates.Add(lastedDate);
                }
                catch (Exception ex)
                {
                    await _logger.ErrorAsync(ErrorMessageHelper.GenerateMessage(ex), ex);
                }
            }

            if (!dates.Any())
            {
                throw new InvalidOperationException($"Can't find lasted importing member date");
            }

            dates.Sort();
            return dates.First();
        }

        private async Task HandleFetchDataAsync(IEnumerable<(DateTime FromDate, DateTime ToDate)> dateRanges)
        {
            foreach (var range in dateRanges)
            {
                await HandleImportDataAsync(range.FromDate, range.ToDate);

                await SaveInsertAffMemberTimeTrackerAsync(range.ToDate);
            }
        }

        private async Task HandleFetchRealDataAsync(IEnumerable<(DateTime FromDate, DateTime ToDate)> dateRanges, DateTime apiServerNow)
        {
            var lastDateRange = dateRanges.LastOrDefault();

            if (lastDateRange.ToDate != DateTime.MinValue)
            {
                await HandleImportDataAsync(lastDateRange.ToDate, apiServerNow.AddSeconds(-5));
            }
        }

        private async Task HandleImportDataAsync(DateTime fromDate, DateTime toDate)
        {
            int pageIndex = 0;
            while (true)
            {
                var data = await _licenseeApi.GetAffCustomerAsync(fromDate, toDate, pageIndex, SettingConstant.MAX_ROW_COUNT);
                if (!data.Any())
                {
                    break;
                }

                var insertMemberCommands = GetImportingData(data);
                foreach (var command in insertMemberCommands)
                {
                    if (SupportedSites.Any(supportSiteCode => string.Compare(supportSiteCode, command.SiteName, StringComparison.OrdinalIgnoreCase) == 0))
                    {
                        await InsertMembers(command, fromDate, toDate);
                    }
                    else
                    {
                        await _logger.WarnAsync($"No Import Setting for Prefix: {command.SettingPrefix} Site: {command.SiteName}, Imported Data  from {fromDate} to {toDate}");
                    }
                }

                pageIndex++;
                if (data.Count() < SettingConstant.MAX_ROW_COUNT)
                {
                    break;
                }
            }
        }

        private async Task SaveInsertAffMemberTimeTrackerAsync(DateTime toDate)
        {
            foreach (var site in SupportedSites)
            {
                var sitePrefixProvider = _licenseeSiteSetting.GetCodePrefix(site);

                var param = new InsertAffMemberTimeTrackerCommand(new PrefixProvider(sitePrefixProvider), _fetchServiceSetting.DataSource)
                {
                    LastImportedTime = toDate,
                    ErrCode = SqlResponseCode.Success,
                    ErrMsg = string.Empty
                };

                await _affMemberRepository.InsertAffMemberTimeTrackerAsync(param);
                if (param.ErrCode != SqlResponseCode.Success)
                {
                    throw new ImporterException(param.ErrCode, param.ErrMsg);
                }
            }
        }

        private async Task InsertMembers(InsertMemberInfoCommand command, DateTime fromDate, DateTime toDate)
        {
            try
            {
                await _affMemberRepository.ImportMemberAsync(command);
                await _logger.InfoAsync($"Prefix: {command.SettingPrefix} Site: {command.SiteName}, Imported Data from {fromDate} to {toDate} by SP {command.GetSettingKey()}: {command.XmlData}");
            }
            catch (Exception ex)
            {
                await _logger.ErrorAsync($"Prefix: {command.SettingPrefix} Site: {command.SiteName} {ex.Message} \n {command.XmlData}", ex);
                throw;
            }
        }

        private List<InsertMemberInfoCommand> GetImportingData(IEnumerable<AffCustomerModel> customerData)
        {
            var commandList = new List<InsertMemberInfoCommand>();
            var groupCustomers = customerData.GroupBy(c => c.SiteName);

            foreach (var group in groupCustomers)
            {
                var importData = GenerateImportedDataBySiteName(group.Key, group.ToList());

                commandList.Add(importData);
            }

            return commandList;
        }

        private InsertMemberInfoCommand GenerateImportedDataBySiteName(string siteName, List<AffCustomerModel> customers)
        {
            var prefixSettingCode = _licenseeSiteSetting.GetCodePrefix(siteName);
            var xml = BuildXml(customers);

            return new InsertMemberInfoCommand(new PrefixProvider(prefixSettingCode))
            {
                SettingPrefix = prefixSettingCode,
                SiteName = siteName,
                XmlData = xml
            };
        }

        private static string BuildXml(List<AffCustomerModel> customers)
        {
            var customerXml = string.Join(
                    string.Empty,
                    customers.Select(customer =>
                    {
                        var builder = new StringBuilder();
                        builder.AppendJoin(" ", "<row"
                            , $"custid=\"{HttpUtility.HtmlEncode(customer?.CustId)}\""
                            , $"username=\"{HttpUtility.HtmlEncode(customer?.UserName)}\""
                            , $"currencyid=\"{HttpUtility.HtmlEncode(customer?.CurrencyId)}\""
                            , $"actstatus=\"{HttpUtility.HtmlEncode(customer?.ActStatus)}\""
                            , $"affid=\"{HttpUtility.HtmlEncode(customer?.AffId)}\""
                            , $"regtime=\"{HttpUtility.HtmlEncode(customer?.RegTime)}\""
                            , $"firstdepositdate=\"{customer?.FirstDepositDate}\""
                            , $"status=\"{HttpUtility.HtmlEncode(customer?.Status)}\""
                            , $"istest=\"{HttpUtility.HtmlEncode(customer?.IsTest)}\""
                            , $"affcode=\"{HttpUtility.HtmlEncode(customer?.AffCode)}\""
                            , $"affmedialinkid=\"{HttpUtility.HtmlEncode(customer?.AffMediaLinkId)}\""
                            , $"clickid=\"{HttpUtility.HtmlEncode(customer?.ClickId)}\""
                            , "/>");
                        return builder.ToString();
                    }));

            return $"<root>{customerXml}</root>";
        }

        private DateTime GetAdjustmentTime(DateTime fromDate)
            => fromDate.AddMinutes(_fetchServiceSetting.AdjustmentValue);

        private async Task<DateTime> GetLastedImportDate(string settingPrefixCode, string sName)
        {
            var prefixProvider = new PrefixProvider(settingPrefixCode);
            return await _affMemberRepository.GetLastedMemberImportedTimeAsync(new MemberImportedTimeInfo(prefixProvider, sName));
        }

        private async Task<string> GetSiteNameAsync(string settingPrefixCode)
        {
            var prefixProvider = new PrefixProvider(settingPrefixCode);

            return await _affMemberRepository.GetSiteNameAsync(new SiteNameInfo(prefixProvider));
        }

        private TimeSpan GetApiMaxTimeRange()
            => TimeSpan.FromMinutes(_fetchServiceSetting.MaxTimeRange);
    }
}
