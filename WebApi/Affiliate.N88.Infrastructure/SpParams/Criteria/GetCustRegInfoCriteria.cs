using Fanex.Data.Repository;

namespace Affiliate.N88.SpParams.Criteria
{
    public class GetCustRegInfoCriteria : CriteriaBase
    {
        public long FromDate { get; }

        public GetCustRegInfoCriteria(long fromDate)
            => FromDate = fromDate;

        public override string GetSettingKey()
            => "API_CustRegInfo_Get";

        public override bool IsValid()
            => true;

    }
}
