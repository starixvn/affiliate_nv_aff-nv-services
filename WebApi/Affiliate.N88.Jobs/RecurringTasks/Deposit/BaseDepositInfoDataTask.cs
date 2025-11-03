using Affiliate.N88.Jobs.Apis;
using Affiliate.N88.Jobs.Configurations;
using Affiliate.N88.Jobs.Constants;
using Affiliate.N88.Models;
using Affiliate.N88.Repository;
using Affiliate.N88.Services;
using Affiliate.N88.Utitlities;
using Fanex.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Affiliate.N88.Jobs.RecurringTasks.Deposit
{
    public abstract class BaseDepositInfoDataTask : FetchDataForMultipleRepositoryTaskBase<DepositInfo>
    {
        private readonly ILicenseeApi _licenseeApi;
        private readonly ILogger _logger;
        private readonly ITrackerService _trackerService;
        protected virtual bool IsNeedRealData => false;

        public BaseDepositInfoDataTask(IThirdPartyImportRepository thirdPartyImportRepository
            , ILicenseeApi licenseeApi
            , ILogger logger
            , ITrackerService trackerService
            , FetchServiceSetting fetchServiceSetting)
            : base(thirdPartyImportRepository, fetchServiceSetting)
        {
            _licenseeApi = licenseeApi;
            _logger = logger;
            _trackerService = trackerService;
        }

        public override async Task FetchDataAsync()
        {
            var tracker = await GetAdjustmentLastTrackerAsync();
            await FetchDataAsync(tracker.UpdateTime);
        }

        private async Task FetchDataAsync(DateTime fromDate)
        {
            var timeRange = GetApiMaxTimeRange();
            var apiServerNow = DateTimeHelper.GetApiServerDateTime();
            var dateRanges = DateTimeHelper.GetProportionTimeRange(fromDate, apiServerNow, timeRange);

            await HandleFetchDataAsync(dateRanges);

            if (IsNeedRealData)
            {
                await HandleFetchRealDataAsync(dateRanges, apiServerNow);
            }
        }

        private async Task HandleFetchDataAsync(IEnumerable<(DateTime FromDate, DateTime ToDate)> dateRanges)
        {
            foreach (var range in dateRanges)
            {
                await HandleImportDataAsync(range.FromDate, range.ToDate);
            }
        }

        protected async Task HandleFetchRealDataAsync(IEnumerable<(DateTime FromDate, DateTime ToDate)> dateRanges, DateTime apiServerNow)
        {
            var lastDateRange = dateRanges.LastOrDefault();

            if (lastDateRange.ToDate != DateTime.MinValue)
            {
                await HandleImportDataAsync(lastDateRange.ToDate, apiServerNow.AddSeconds(-5), false);
            }
        }

        private async Task HandleImportDataAsync(DateTime fromDate, DateTime toDate, bool isNeedImportTracker = true)
        {
            int pageIndex = 0;
            while (true)
            {
                var data = await _licenseeApi.GetDepositInfo(fromDate, toDate, pageIndex, SettingConstant.MAX_ROW_COUNT);
                if (!data.Any())
                {
                    break;
                }
                await ImportDataToMultipleRepoAsync(data);
                pageIndex++;

                if (isNeedImportTracker)
                {
                    var trackerItem = new TrackerItem
                    {
                        Id = data.Max(_ => _.DepositId),
                        UpdateTime = data.Max(_ => _.UpdateTime.Value),
                    };
                    await _trackerService.SaveTrackerAsync(TableName,
                        new TrackerInfo
                        {
                            CalledTime = DateTime.Now,
                            KeyValue = JsonConvert.SerializeObject(trackerItem)
                        });
                }

                if (data.Count() < SettingConstant.MAX_ROW_COUNT)
                {
                    break;
                }
            }
        }

        private async Task<TrackerItem> GetAdjustmentLastTrackerAsync()
        {
            var tracker = await _trackerService.GetTrackerAsync(TableName, t => t.CalledTime);
            var trackerItem = JsonConvert.DeserializeObject<TrackerItem>(tracker.KeyValue);
            trackerItem.UpdateTime = trackerItem.UpdateTime.AddMinutes(Settings.AdjustmentValue);
            return trackerItem;
        }

        protected override string BuildXml(IEnumerable<DepositInfo> data)
             => XmlHelper.BuildXml(data, "root", "row");

        protected override IEnumerable<string> GetSupportedRepoPrefix()
            => Settings.SupportedPrefix;

        private TimeSpan GetApiMaxTimeRange()
            => TimeSpan.FromMinutes(Settings.MaxTimeRange);

        private sealed class TrackerItem
        {
            public int Id { get; set; }

            public DateTime UpdateTime { get; set; }
        }
    }
}
