using Fanex.Data.Repository;

namespace Affiliate.N88.SpParams.Criteria
{
    public class GetTpGameTypeCriteria : CriteriaBase
    {
        public override string GetSettingKey() => "N88_Get_TpGameType";

        public override bool IsValid() => true;
    }
}
