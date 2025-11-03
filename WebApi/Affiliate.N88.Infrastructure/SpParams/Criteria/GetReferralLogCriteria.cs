using Fanex.Data.Repository;

namespace Affiliate.N88.SpParams.Criteria
{
    public class GetReferralLogCriteria : CriteriaBase
    {
        public int Id { get; }
        public int BatchCount { get; }

        public GetReferralLogCriteria(int id, int batchCount)
        {
            Id = id;
            BatchCount = batchCount;
        }

        public override string GetSettingKey() => "API_ReferralLog_Get";

        public override bool IsValid() => true;
    }
}