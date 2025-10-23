using Affiliate.N88.Jobs.Apis;
using Affiliate.N88.Jobs.Configurations;
using Affiliate.N88.Jobs.Constants;
using Affiliate.N88.Jobs.RecurringTasks.Interfaces;
using Affiliate.N88.Models;
using Affiliate.N88.Repository;
using Affiliate.N88.Services;
using Affiliate.N88.Utitlities;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Affiliate.N88.Jobs.RecurringTasks
{
    public interface IFetchCustVerificationRewardDataTask : IFetchDataTask { }

    public class FetchCustVerificationRewardDataTask : FetchDataForMultipleRepositoryTaskBase<CustVerificationReward>, IFetchCustVerificationRewardDataTask
    {
        private readonly ITrackerService _timeTrackerService;
        private readonly ILicenseeApi _liceseeApi;

        private const string PHONE_REWARD_FIELD = "PhoneRewardTime";
        private const string EMAIL_REWARD_FIELD = "EmailRewardTime";
        private const int MAX_BATCH_COUNT = 2000;

        public FetchCustVerificationRewardDataTask(ILicenseeApi licenseeApi,
            IThirdPartyImportRepository _thirdPartyRepo,
            ITrackerService timeTrackerService,
            IOptionsSnapshot<FetchServiceSetting> configuration)
            : base(_thirdPartyRepo, configuration.Get(Constants.ServiceConfigName.PULL_CUST_VERIFICATION_REWARD_SETTING))
        {
            _timeTrackerService = timeTrackerService ?? throw new ArgumentNullException(nameof(timeTrackerService));
            _liceseeApi = licenseeApi ?? throw new ArgumentNullException(nameof(licenseeApi));
        }

        public override async Task FetchDataAsync()
        {
            var fromDate = await GetLastTrackingTimeAsync();
            await FetchDataAsync(fromDate);
        }

        private async Task FetchDataAsync(DateTime fromDate)
        {
            var emailTrackerTime = await ImportAndGetEmailRewardTrackTime(fromDate);
            var phoneTrackeTime = await ImportAndGetPhoneRewardTrackTime(fromDate);
            var minTrackerTime = emailTrackerTime <= phoneTrackeTime ? emailTrackerTime : phoneTrackeTime;
            if (minTrackerTime != DateTime.MinValue)
            {
                await _timeTrackerService.SaveTrackerAsync(TableName, new TrackerInfo
                {
                    CalledTime = DateTime.Now,
                    KeyValue = minTrackerTime.ToString(Format.DateTimeFormat)
                });
            }
        }

        private async Task<DateTime> ImportAndGetEmailRewardTrackTime(DateTime fromDate)
            => await ImportAndGetTrackTime(fromDate, EMAIL_REWARD_FIELD, _ => _.EmailRewardTime.GetValueOrDefault());

        private async Task<DateTime> ImportAndGetPhoneRewardTrackTime(DateTime fromDate)
            => await ImportAndGetTrackTime(fromDate, PHONE_REWARD_FIELD, _ => _.PhoneRewardTime.GetValueOrDefault());

        private async Task<DateTime> ImportAndGetTrackTime(DateTime fromDate, string conditionFieldName, Func<CustVerificationReward, DateTime> trackerSelector)
        {
            var queryDate = fromDate;
            var isFirstTimeQuery = true;
            DateTime trackerDate = DateTime.MinValue;
            var data = Enumerable.Empty<CustVerificationReward>();
            while (isFirstTimeQuery || data.Count() == MAX_BATCH_COUNT)
            {
                isFirstTimeQuery = false;
                data = await _liceseeApi.GetCustVerificationRewards(queryDate, conditionFieldName, MAX_BATCH_COUNT);
                if (data.Any())
                {
                    await ImportDataToMultipleRepoAsync(data);
                    trackerDate = data.Max(trackerSelector);
                    queryDate = trackerDate;
                }
            }
            return trackerDate;
        }

        private async Task<DateTime> GetLastTrackingTimeAsync()
        {
            var tracker = await _timeTrackerService.GetTrackerAsync(TableName, t => Convert.ToDateTime(t.KeyValue));
            var date =  Convert.ToDateTime(tracker.KeyValue);
            var adjustDate = date.AddMinutes(Settings.AdjustmentValue);
            return adjustDate;
        }

        protected override string BuildXml(IEnumerable<CustVerificationReward> data)
            => XmlHelper.BuildXml(data, "root", "row");

        protected override IEnumerable<string> GetSupportedRepoPrefix()
            => Settings.SupportedPrefix;
    }
}
