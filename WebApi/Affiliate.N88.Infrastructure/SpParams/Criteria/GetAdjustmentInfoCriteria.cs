using Fanex.Data.Repository;

namespace Affiliate.N88.SpParams.Criteria
{
    public class GetAdjustmentInfoCriteria : CriteriaBase
    {
        public GetAdjustmentInfoCriteria(int id, int batchCount)
        {
            Id = id;
            BatchCount = batchCount;
        }

        public int Id { get; set; }

        public int BatchCount { get; set; }

        public override string GetSettingKey()
            => "API_AdjustmentInfo_Get";

        public override bool IsValid() => true;
    }
}
