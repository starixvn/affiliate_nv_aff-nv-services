using System;

namespace Affiliate.N88.Params
{
    public class BatchCountParam
    {
        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public int BatchCount { get; set; }

        public long SQ { get; set; }

        public DateTime CreateTime { get; set; }
    }
}
