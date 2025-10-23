using Fanex.Data.Repository;

namespace Affiliate.N88.SpParams.Criteria
{
    public class GetCustWalletCriteria : CriteriaBase
    {
        public GetCustWalletCriteria(DateTime fromDate) => FromDate = fromDate;

        public override string GetSettingKey() => "N88_Get_CustWallet";

        public override bool IsValid() => true;

        public DateTime FromDate { get; set; }
    }
}
