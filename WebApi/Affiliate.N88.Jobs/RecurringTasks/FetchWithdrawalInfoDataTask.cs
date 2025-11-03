using Affiliate.N88.Constants;
using Affiliate.N88.Jobs.Apis;
using Affiliate.N88.Jobs.Configurations;
using Affiliate.N88.Jobs.Constants;
using Affiliate.N88.Jobs.RecurringTasks.Interfaces;
using Affiliate.N88.Models;
using Affiliate.N88.Repository;
using Affiliate.N88.Services;
using Fanex.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Affiliate.N88.Jobs.RecurringTasks
{
    public interface IFetchWithdrawalInfoDataTask : IFetchDataTask
    { }

    public class FetchWithdrawalInfoDataTask : FetchDataByBatchForMultiRepoTaskBase<Withdrawal, string>, IFetchWithdrawalInfoDataTask
    {
        private ILicenseeApi Api { get; }
        private ILogger Logger { get; }

        public FetchWithdrawalInfoDataTask(IThirdPartyImportRepository thirdPartyImportRepository,
            ITrackerService trackerService,
            ILogger logger,
            ILicenseeApi licenseeApi,
            IOptionsSnapshot<FetchServiceSetting> configuration)
            : base(thirdPartyImportRepository, configuration.Get(Constants.ServiceConfigName.PULL_WITHDRAWAL_INFO), trackerService)
        {
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            Api = licenseeApi;
        }

        protected override async Task<IEnumerable<Withdrawal>> GetDataAsync(string trackValue, int batchCount)
        {
            var result = await Api.GetWithdrawalInfo(Convert.ToDateTime(trackValue), batchCount);
            return result;
        }

        protected override string GetInsertTrackingValue(IEnumerable<Withdrawal> data)
        {
            var trackValue = data.Max(d => d.UpdateTime);

            if (trackValue.HasValue)
            {
                return trackValue.Value.ToString(Format.DateTimeFormat);
            }
            throw new ArgumentException($"Can not insert nullable DateTime Tracker Value of table {TableName}");
        }

        protected override async Task<string> GetLastTrackingValueAsync()
        {
            var tracker = await _timeTrackerService.GetTrackerAsync(TableName, t => t.CalledTime);
            var date = string.IsNullOrWhiteSpace(tracker.KeyValue)
                ? throw new ArgumentException($"Not valid DateTime Tracker Value {tracker.KeyValue} of table {TableName}")
                : Convert.ToDateTime(tracker.KeyValue);

            var adjustDateTime = GetAdjustmentTime(date);
            return adjustDateTime.ToString(Format.DateTimeFormat);
        }

        private DateTime GetAdjustmentTime(DateTime date) => date.AddMinutes(Settings.AdjustmentValue);
    }
}