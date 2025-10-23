using Affiliate.N88.DTO;
using Affiliate.N88.SpParams.Commands;

namespace Affiliate.N88.Repository
{
    public interface IAffMemberRepository
    {
        Task ImportMemberAsync(InsertMemberInfoCommand memberInfo);

        Task<DateTime> GetLastedMemberImportedTimeAsync(MemberImportedTimeInfo settingInfo);

        Task<string> GetSiteNameAsync(SiteNameInfo settingInfo);

        Task InsertAffMemberTimeTrackerAsync(InsertAffMemberTimeTrackerCommand timeTracker);
    }
}