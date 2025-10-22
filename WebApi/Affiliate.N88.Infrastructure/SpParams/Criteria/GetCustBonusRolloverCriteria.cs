using Fanex.Data.Repository;


namespace Affiliate.N88.SpParams.Criteria
{
    public class GetCustBonusRolloverCriteria : CriteriaBase
    {
        public GetCustBonusRolloverCriteria(int id, int batchCount)
        {
            Id = id;
            BatchCount = batchCount;
        }

        public int Id { get; set; }

        public int BatchCount { get; set; }

        public override string GetSettingKey() => "API_CustBonusRollover_Get";

        public override bool IsValid() => true;
    }
}
