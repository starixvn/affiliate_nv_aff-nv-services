using Affiliate.N88.Jobs.Configurations;
using Affiliate.N88.Jobs.Constants;
using Affiliate.N88.Models;
using Affiliate.N88.Repository;
using Affiliate.N88.Services;
using Affiliate.N88.Utitlities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Affiliate.N88.Jobs.RecurringTasks
{
    public abstract class FetchDataByBatchForMultiRepoTaskBase<TData, TTracker> : FetchDataForMultipleRepositoryTaskBase<TData>
        where TData : class
    {
        protected readonly ITrackerService _timeTrackerService;

        protected override int PagingSize => SettingConstant.MAX_ROW_COUNT;


        protected FetchDataByBatchForMultiRepoTaskBase(IThirdPartyImportRepository thirdPartyImportRepository, FetchServiceSetting setting, ITrackerService trackerService) 
            : base(thirdPartyImportRepository, setting)
        {
            _timeTrackerService = trackerService;
        }

        public override async Task FetchDataAsync()
        {
            var queryValue = await GetLastTrackingValueAsync();
            var data = await GetDataAsync(queryValue, PagingSize);

            await ImportDataByBatchAsync(data);
        }

        protected virtual async Task ImportDataByBatchAsync(IEnumerable<TData> data)
        {
            var importData = data;
            while (importData != null && importData.Any())
            {
                await ImportDataToMultipleRepoAsync(importData);
                var trackValue = GetInsertTrackingValue(importData);
                var keyValue = trackValue is string ? trackValue as string : JsonConvert.SerializeObject(trackValue);

                await _timeTrackerService.SaveTrackerAsync(TableName,
                    new TrackerInfo
                    {
                        CalledTime = DateTime.Now,
                        KeyValue = keyValue
                    });

                if (importData.Count() == PagingSize)
                {
                    importData = await GetDataAsync(trackValue, PagingSize);
                }
                else
                {
                    break;
                }
            }
        }

        protected abstract Task<IEnumerable<TData>> GetDataAsync(TTracker trackValue, int batchCount);

        protected abstract TTracker GetInsertTrackingValue(IEnumerable<TData> data);

        protected abstract Task<TTracker> GetLastTrackingValueAsync();

        protected override string BuildXml(IEnumerable<TData> data)
             => XmlHelper.BuildXml(data, "root", "row");

        protected override IEnumerable<string> GetSupportedRepoPrefix()
            => Settings.SupportedPrefix;
    }
}
