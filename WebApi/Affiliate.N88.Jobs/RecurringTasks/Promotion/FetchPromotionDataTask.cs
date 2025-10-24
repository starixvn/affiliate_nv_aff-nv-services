using Affiliate.N88.Constants;
using Affiliate.N88.Jobs.Apis;
using Affiliate.N88.Jobs.Configurations;
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

namespace Affiliate.N88.Jobs.RecurringTasks.Promotion
{
    public interface IFetchPromotionDataTask : IFetchDataTask
    {
        Task FetchDataDailyTask();
    }

    public class FetchPromotionDataTask : FetchDataForMultipleRepositoryTaskBase<PromotionModel>, IFetchPromotionDataTask
    {
        private IPromotionApi Api { get; }
        private ILogger Logger { get; }
        private ITrackerService TrackerService { get; }

        private const int API_SERVER_TIME_OFFSET = 0;

        public FetchPromotionDataTask(IPromotionApi licenseeApi,
            IThirdPartyImportRepository _thirdPartyRepo,
            ILogger logger,
            ITrackerService timeTrackerService,
            IOptionsSnapshot<FetchServiceSetting> configuration)
            : base(_thirdPartyRepo, configuration.Get(Constants.ServiceConfigName.PULL_PROMOTION_SETTING))
        {
            Api = licenseeApi;
            Logger = logger;
            TrackerService = timeTrackerService;
        }

        public override async Task FetchDataAsync()
        {
            var tracker = await TrackerService.GetTrackerAsync(TableName, t => t.CalledTime);
            var fromDate = DateTime.Parse(tracker.KeyValue);
            var toDate = GetAdjustmentTime();
            await FetchDataAndTrackTimeAsync(fromDate, toDate);
        }

        public async Task FetchDataDailyTask()
        {
            var fromDate = GetAdjustmentApiServerDate(new TimeSpan(-1, 0, 0, 0));
            var toDate = GetAdjustmentTime();
            var intervalTime = GetApiMaxTimeRange();
            var timeRanges = DateTimeHelper.GetProportionTimeRange(fromDate, toDate, intervalTime);
            foreach (var range in timeRanges)
            {
                try
                {
                    await FetchAndInsertDataAsync(range.FromDate, range.ToDate);
                }
                catch
                {
                    Logger.Error($"Excetion when fetch {TableName} from {range.FromDate} to {range.ToDate}");
                    throw;
                }
            }
        }

        private async Task FetchDataAndTrackTimeAsync(DateTime fromDate, DateTime toDate)
        {
            var intervalTime = GetApiMaxTimeRange();
            var timeRanges = DateTimeHelper.GetProportionTimeRange(fromDate, toDate, intervalTime);
            foreach (var range in timeRanges)
            {
                try
                {
                    await FetchAndInsertDataAsync(range.FromDate, range.ToDate);
                    var tracker = new TrackerInfo { CalledTime = DateTimeOffset.UtcNow.DateTime, KeyValue = range.ToDate.ToString(Format.DateTimeFormat) };
                    await TrackerService.SaveTrackerAsync(TableName, tracker);
                }
                catch
                {
                    Logger.Error($"Excetion when fetch {TableName} from {range.FromDate} to {range.ToDate}");
                    throw;
                }
            }
        }

        private async Task FetchAndInsertDataAsync(DateTime fromDate, DateTime toDate)
        {
            var datas = await Api.GetPromotionsAsync(fromDate, toDate);
            await ImportDataToMultipleRepoAsync(datas);
        }

        private DateTime GetAdjustmentTime() => GetAdjustmentApiServerDate(new TimeSpan(0, Settings.AdjustmentValue, 0));

        protected override string BuildXml(IEnumerable<PromotionModel> data)
            => XmlHelper.BuildXml(data, "root", "row");

        protected override IEnumerable<string> GetSupportedRepoPrefix()
            => Settings.SupportedPrefix;

        private TimeSpan GetApiMaxTimeRange() => TimeSpan.FromMinutes(Settings.MaxTimeRange);

        private DateTime GetAdjustmentApiServerDate(TimeSpan ajustmentTime)
        {
            var date = DateTimeOffset.Now.ToOffset(TimeSpan.FromHours(API_SERVER_TIME_OFFSET));
            var adjustDate = date.Add(ajustmentTime);
            return adjustDate.DateTime;
        }
    }
}