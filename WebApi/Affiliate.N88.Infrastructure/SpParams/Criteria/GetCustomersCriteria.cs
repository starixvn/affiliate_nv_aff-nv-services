using Fanex.Data.Repository;

namespace Affiliate.N88.SpParams.Criteria
{
    public class GetCustomersCriteria : CriteriaBase
    {
        public GetCustomersCriteria(DateTime fromDate, DateTime toDate, int pageIndex, int pageSize)
        {
            FromDate = fromDate;
            ToDate = toDate;
            PageIndex = pageIndex;
            PageSize = pageSize;
        }

        public override string GetSettingKey() => "N88_Get_Customer";

        public override bool IsValid() => true;

        public DateTime FromDate { get; }

        public DateTime ToDate { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; }
    }
}
