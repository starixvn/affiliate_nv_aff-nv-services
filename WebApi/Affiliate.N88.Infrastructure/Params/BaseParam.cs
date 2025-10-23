namespace Affiliate.N88.Params
{
    public class BaseParam
    {
        public long SQ { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }
    }
}
