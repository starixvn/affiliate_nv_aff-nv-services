using Affiliate.N88.Constants;
using Affiliate.N88.Jobs.Apis;
using Affiliate.N88.Jobs.Configurations;
using Affiliate.N88.Jobs.Constants;
using Affiliate.N88.Jobs.RecurringTasks.Interfaces;
using Affiliate.N88.Models;
using Affiliate.N88.Repository;
using Affiliate.N88.Services;
using Affiliate.N88.Utitlities;
using Fanex.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Affiliate.N88.Jobs.RecurringTasks
{
    public interface IFetchCustomerWalletDataTask : IFetchDataTask
    { }

    public class FetchCustomerWalletDataTask : FetchDataForMultipleRepositoryTaskBase<CustWalletInfo>, IFetchCustomerWalletDataTask
    {
        private readonly ITrackerService _trackerService;
        private readonly ILicenseeApi _licenseeApi;
        private readonly ILogger _logger;

        public FetchCustomerWalletDataTask(IThirdPartyImportRepository thirdPartyImportRepository
            , ILicenseeApi licenseeApi
            , ILogger logger
            , ITrackerService trackerService
            , IOptionsSnapshot<FetchServiceSetting> setting)
            : base(thirdPartyImportRepository, setting.Get(ServiceConfigName.PULL_CUST_WALLET_SETTING))
        {
            _trackerService = trackerService;
            _licenseeApi = licenseeApi;
            _logger = logger;
        }

        public override async Task FetchDataAsync()
        {
            var tracker = await _trackerService.GetTrackerAsync(TableName, t => t.CalledTime);
            var fromDate = DateTime.Parse(tracker.KeyValue);
            await FetchDataAsync(fromDate);
        }

        public async Task FetchDataAsync(DateTime fromDate)
        {
            var toDate = GetAdjustmentTime();
            var data = await _licenseeApi.GetCustomerWalletAsync(fromDate);
            await ImportDataToMultipleRepoAsync(data);
            var tracker = new TrackerInfo { CalledTime = DateTimeOffset.Now.DateTime, KeyValue = toDate.ToString(Format.DateTimeFormat) };
            await _trackerService.SaveTrackerAsync(TableName, tracker);
        }

        private DateTime GetAdjustmentTime()
            => DateTimeOffset.Now.DateTime.AddMinutes(Settings.AdjustmentValue);

        protected override string BuildXml(IEnumerable<CustWalletInfo> data)
            => XmlHelper.BuildXml(data, "root", "row");

        protected override IEnumerable<string> GetSupportedRepoPrefix()
            => Settings.SupportedPrefix;
    }
}