using Affiliate.N88.Constants;
using Fanex.Data.Repository;

namespace Affiliate.N88.SpParams.Criteria
{
    public class GetDepositInfoCriteria : CriteriaBase
    {
        public GetDepositInfoCriteria(DateTime fromDate, DateTime toDate, int pageIndex, int pageSize = PagingConstants.DefaultPageSize)
        {
            FromDate = fromDate;
            ToDate = toDate;
            PageIndex = pageIndex;
            PageSize = pageSize;
        }

        public override string GetSettingKey() 
            => "N88_Get_DepositInfo";

        public override bool IsValid()
            => true;

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }
    }
}
