using Affiliate.N88.Constants;
using Affiliate.N88.Jobs.Apis;
using Affiliate.N88.Jobs.Configurations;
using Affiliate.N88.Jobs.RecurringTasks.Interfaces;
using Affiliate.N88.Models;
using Affiliate.N88.Repository;
using Affiliate.N88.Services;
using Affiliate.N88.Utitlities;
using Fanex.Logging;
using Hangfire;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Affiliate.N88.Jobs.RecurringTasks
{
    public interface IFetchVipUpgradeReviewDataTask : IFetchDataTask
    {
        [DisableConcurrentExecution(60)]
        [AutomaticRetry(OnAttemptsExceeded = AttemptsExceededAction.Delete, Attempts = 1)]
        Task FetchLastMonthDataAsync();
    }

    public class FetchVipUpgradeReviewDataTask : FetchDataForMultipleRepositoryTaskBase<VipUpgradeReviewInfo>, IFetchVipUpgradeReviewDataTask
    {
        private readonly ILicenseeApi _licenseeApi;
        private readonly ILogger _logger;
        private readonly ITrackerService _timeTrackerService;

        public FetchVipUpgradeReviewDataTask(ILicenseeApi licenseeApi,
             IThirdPartyImportRepository _thirdPartyRepo,
             ILogger logger,
             ITrackerService timeTrackerService,
             IOptionsSnapshot<FetchServiceSetting> configuration)
             : base(_thirdPartyRepo, configuration.Get(Constants.ServiceConfigName.PULL_VIP_UPGRADE_REVIEW_SETTING))
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _licenseeApi = licenseeApi ?? throw new ArgumentNullException(nameof(licenseeApi));
            _timeTrackerService = timeTrackerService ?? throw new ArgumentNullException(nameof(timeTrackerService));
        }

        public async Task FetchLastMonthDataAsync()
        {
            var now = DateTime.Now;
            var lastMonth = now.AddMonths(-1);
            var lastMonthDate = new DateTime(lastMonth.Year, lastMonth.Month, 1);

            await FetchDataAsync(lastMonthDate);
        }

        public override async Task FetchDataAsync()
        {
            var tracker = await _timeTrackerService.GetTrackerAsync(TableName, t => t.CalledTime);
            var trackDate = DateTime.Parse(tracker.KeyValue);
            var monthDate = new DateTime(trackDate.Year, trackDate.Month, 1);
            await FetchDataAsync(monthDate);
        }

        public async Task FetchDataAsync(DateTime monthDate)
        {
            var fromDate = monthDate;
            var toDate = GetAdjustmentTime();
            while (fromDate <= toDate)
            {
                var datas = await FetchDataFromMonthAsync(fromDate);
                foreach (var data in datas)
                {
                    if (data.Any())
                    {
                        await ImportDataToMultipleRepoAsync(data);
                    }
                }
                var tracker = new TrackerInfo { CalledTime = DateTimeOffset.Now.DateTime, 
                    KeyValue = fromDate.ToString(Format.DateTimeFormat) };
                await _timeTrackerService.SaveTrackerAsync(TableName, tracker);
                fromDate = fromDate.AddMonths(1);
            }
        }

        private async Task<IEnumerable<IEnumerable<VipUpgradeReviewInfo>>> FetchDataFromMonthAsync(DateTime fromDate)
        {
            var datas = new List<IEnumerable<VipUpgradeReviewInfo>>();
            var pageIndex = 0;
            var pageSize = 2000;
            var data = await _licenseeApi.GetVipUpgradeReviews(fromDate, pageIndex, pageSize);
            datas.Add(data);
            while (data.Count() == pageSize)
            {
                pageIndex++;
                data = await _licenseeApi.GetVipUpgradeReviews(fromDate, pageIndex, pageSize);
                datas.Add(data);
            }
            return datas;
        }

        private DateTime GetAdjustmentTime()
        {
            var now = DateTime.Now;
            var monthDate = new DateTime(now.Year, now.Month, now.Day);
            var adjustDate = monthDate.AddDays(Settings.AdjustmentValue);
            return new DateTime(adjustDate.Year, adjustDate.Month, 1);
        }

        protected override string BuildXml(IEnumerable<VipUpgradeReviewInfo> data)
            => XmlHelper.BuildXml(data, "root", "row");

        protected override IEnumerable<string> GetSupportedRepoPrefix()
            => Settings.SupportedPrefix;
    }
}