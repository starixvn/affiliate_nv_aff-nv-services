using System;

namespace Affiliate.N88.Models
{
    public class TpTransferInfo
    {
        public int TransferId { get; set; }

        public string? TransId { get; set; }

        public int? TpId { get; set; }

        public int? CustId { get; set; }

        public string? UserName { get; set; }

        public int? SiteId { get; set; }

        public int? Direction { get; set; }

        public decimal? Amount { get; set; }

        public string? Memo { get; set; }

        public int? Status { get; set; }

        public DateTime? CreateTime { get; set; }

        public string? ThrirdId { get; set; }
    }
}
