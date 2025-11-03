using Affiliate.N88.Core;
using Affiliate.N88.Jobs.Apis;
using Affiliate.N88.Jobs.Configurations;
using Affiliate.N88.Jobs.Constants;
using Affiliate.N88.Jobs.RecurringTasks.Interfaces;
using Affiliate.N88.Params;
using Affiliate.N88.Services;
using Fanex.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Affiliate.N88.Jobs.RecurringTasks.RetryJob
{
    public interface IReInsertAffiliateInfoDataTask : IRetryDataTask { }

    public class ReInsertAffiliateInfoDataTask : IReInsertAffiliateInfoDataTask
    {
        private readonly IDataTrackerService _trackerService;
        private readonly ILicenseeApi _licenseeApi;
        private readonly ILogger _logger;

        private static readonly IEnumerable<int> ListPullStatusId = new int[] { DataTrackerStatus.InProcess, DataTrackerStatus.Failure };
        
        private FetchServiceSetting Setting { get; }

        public ReInsertAffiliateInfoDataTask( IDataTrackerService dataTrackerService,
            ILicenseeApi licenseeApi,
            IOptionsSnapshot<FetchServiceSetting> configuration,
            ILogger logger
            ) 
        {
            _trackerService = dataTrackerService;
            _licenseeApi = licenseeApi;
            _logger = logger;
            Setting = configuration.Get(ServiceConfigName.INSERT_AFFILIATE_SETTING);
        }

        public async Task HandleAsync()
        {
            var prefixPrviders = Setting.SupportedPrefix.Select(_ => new PrefixProvider(_));
            var dataTrackerItems = await _trackerService.FetchTrackerAsync(
                prefixPrviders, 
                new int[] { DataTrackerType.InsertAffiliate }, 
                ListPullStatusId
            );

            foreach (var item in dataTrackerItems)
            {
                var affiliateInfo = JsonConvert.DeserializeObject<AffiliateInfoParam>(item.PushDataInfo);
                var result = await _licenseeApi.InsertAffiliateInfo(affiliateInfo);

                if (result.IsSuccess)
                {
                    await _trackerService.UpdateTrackerStatusAsync(item, DataTrackerStatus.Success);
                    await _logger.InfoAsync($"Insert affiliate {affiliateInfo.AffiliateName}, SiteId {affiliateInfo.SiteId} to B2C DB:\n" +
                        $"Item tracker: {JsonConvert.SerializeObject(item)}\n" +
                        $"Affiliate Info: {JsonConvert.SerializeObject(affiliateInfo)}");
                }
                else
                {
                    _logger.Error($"Insert affiliate {affiliateInfo.AffiliateName}, SiteId {affiliateInfo.SiteId} to B2C DB fail:\n" +
                        $"Item tracker: {JsonConvert.SerializeObject(item)}\n" +
                        $"Result: {JsonConvert.SerializeObject(result)}");
                }
            }
        }
    }
}
