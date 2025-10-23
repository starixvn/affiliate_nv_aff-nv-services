using Fanex.Data.Repository;

namespace Affiliate.N88.SpParams.Criteria
{
    public class GetReferralMLogCriteria : CriteriaBase
    {
        public int Id { get; }
        public int BatchCount { get; }

        public GetReferralMLogCriteria(int id, int batchCount)
        {
            Id = id;
            BatchCount = batchCount;
        }

        public override string GetSettingKey() => "API_ReferralMLog_Get";

        public override bool IsValid() => true;
    }
}
