using Fanex.Data.Repository;

namespace Affiliate.N88.SpParams.Criteria
{
    public class GetCustBalanceCriteria : CriteriaBase
    {
        public GetCustBalanceCriteria(string custIdList) => CustIdList = custIdList;

        public override string GetSettingKey() => "API_BalanceInfo_Get";

        public override bool IsValid() => true;

        public string? CustIdList { get; set; }
    }
}
