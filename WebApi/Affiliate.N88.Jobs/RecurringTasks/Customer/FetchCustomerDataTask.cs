using Affiliate.N88.Jobs.Apis;
using Affiliate.N88.Jobs.Configurations;
using Affiliate.N88.Jobs.Constants;
using Affiliate.N88.Jobs.RecurringTasks.Interfaces;
using Affiliate.N88.Repository;
using Affiliate.N88.Services;
using Affiliate.N88.Utitlities;
using Fanex.Logging;
using Hangfire;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Affiliate.N88.Jobs.RecurringTasks.Customer
{
    public interface IFetchCustomerDataTask : IFetchDataTask
    {
        [DisableConcurrentExecution(60)]
        [AutomaticRetry(OnAttemptsExceeded = AttemptsExceededAction.Fail, Attempts = 1)]
        Task FetchDataDailyTask();

        [DisableConcurrentExecution(60)]
        [AutomaticRetry(OnAttemptsExceeded = AttemptsExceededAction.Fail, Attempts = 1)]
        Task FetchDataByDateRangeTask();

        [DisableConcurrentExecution(60)]
        [AutomaticRetry(OnAttemptsExceeded = AttemptsExceededAction.Fail, Attempts = 1)]
        Task FetchFixedDataTask();
    }

    public sealed class FetchCustomerDataTask : BaseCustomerDataTask, IFetchCustomerDataTask
    {
        private const int DATE_RANGE = 1440;

        public FetchCustomerDataTask(IThirdPartyImportRepository thirdPartyImportRepository
            , ILicenseeApi licenseeApi
            , ILogger logger
            , ITrackerService timeTrackerService
            , IOptionsSnapshot<FetchServiceSetting> setting)
            : base(thirdPartyImportRepository, licenseeApi, logger, timeTrackerService, setting.Get(ServiceConfigName.PULL_CUSTOMER_SETTING))
        {
        }

        public async Task FetchDataByDateRangeTask()
        {
            var maxTimeRange = TimeSpan.FromMinutes(DATE_RANGE);

            await FetchDataAsync(maxTimeRange);
        }

        public async Task FetchDataDailyTask()
        {
            var now = DateTimeHelper.GetApiServerDateTime();
            var adjustmentDay = now.AddDays(-1);
            var fromDate = new DateTime(adjustmentDay.Year, adjustmentDay.Month, adjustmentDay.Day, 0, 0, 0);
            var maxTimeRange = GetApiMaxTimeRange();
            var dateRanges = DateTimeHelper.GetProportionTimeRange(fromDate, now, maxTimeRange);
            foreach (var range in dateRanges)
            {
                var pageIndex = 0;
                while (true)
                {
                    var data = await _liceseeApi.GetCustomersAsync(range.FromDate, range.ToDate, pageIndex, SettingConstant.MAX_ROW_COUNT);
                    await ImportDataToMultipleRepoAsync(data);
                    pageIndex++;
                    if (data.Count() < SettingConstant.MAX_ROW_COUNT)
                    {
                        break;
                    }
                }
            }
        }

        public async Task FetchFixedDataTask()
        {
            var data = await _liceseeApi.GetFixedCustomers();
            await ImportDataToMultipleRepoAsync(data);
        }
    }
}