

using Affiliate.N88.DTOs;
using Affiliate.N88.SpParams;
using Affiliate.N88.SpParams.Commands;
using Fanex.Data.Repository;

namespace Affiliate.N88.Repository
{
    public class DataTrackerRepository : IDataTrackerRepository
    {
        private readonly IDynamicRepository _repo;

        public DataTrackerRepository(IDynamicRepository repo)
        {
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
        }

        public Task<IEnumerable<DataTrackerItem>> FetchDataTracker(DataTrackerCriteria criteria)
           => _repo.FetchAsync<DataTrackerItem>(criteria);
        

        public Task UpdateDataTrackerAsync(UpdateDataTrackerCommand command)
            => _repo.ExecuteAsync(command);
    }
}
