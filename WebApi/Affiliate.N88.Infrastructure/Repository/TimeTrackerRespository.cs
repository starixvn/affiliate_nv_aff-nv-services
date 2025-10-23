using Affiliate.N88.Models;
using Affiliate.N88.SpParams.Commands;
using Affiliate.N88.SpParams.Criteria;
using Fanex.Data.Repository;

namespace Affiliate.N88.Repository
{
    public class TrackerRespository : ITrackerRepository
    {
        private readonly IDynamicRepository _repo;

        public TrackerRespository(IDynamicRepository repo)
        {
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
        }

        public async Task<TrackerInfo> GetLastedTrackerAsync(GetLastedImportTimeCriteria timeInfo)
            => await _repo.GetAsync<TrackerInfo>(timeInfo);

        public async Task ImportTimeTrackerAsync(InsertImportTrackerCommand timeInfo)
            => await _repo.ExecuteAsync(timeInfo);
    }
}
