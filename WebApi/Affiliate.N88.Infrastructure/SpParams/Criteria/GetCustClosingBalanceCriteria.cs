using Fanex.Data.Repository;

namespace Affiliate.N88.SpParams.Criteria
{
    public class GetCustClosingBalanceCriteria : CriteriaBase
    {
        public GetCustClosingBalanceCriteria(DateTime lastGetTime, int pageIndex, int pageSize)
        {
            LastGetTime = lastGetTime;
            PageIndex = pageIndex;
            PageSize = pageSize;
        }

        public override string GetSettingKey() => "API_CustClosingBalance_Get";

        public override bool IsValid() => true;

        public DateTime LastGetTime { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }
    }
}
