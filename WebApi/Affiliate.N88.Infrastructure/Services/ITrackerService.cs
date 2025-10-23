using Affiliate.N88.Models;

namespace Affiliate.N88.Services
{
    public interface ITrackerService
    {
        Task<TrackerInfo> GetTrackerAsync<TKey>(string source, Func<TrackerInfo, TKey> minKeySelector);

        Task<TrackerInfo> GetTrackerAsync<TKey>(string source, Func<TrackerInfo, TKey> minKeySelector, IComparer<TKey> comparer);

        Task SaveTrackerAsync(string source, TrackerInfo tracker);
    }
}