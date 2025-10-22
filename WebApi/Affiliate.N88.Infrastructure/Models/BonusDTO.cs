using System;

namespace Affiliate.N88.Models
{
    public class BonusDTO
    {
        public int BonusId { get; set; }

        public string? TransId { get; set; }

        public int PromotionType { get; set; }

        public string? BonusCode { get; set; }

        public int CustId { get; set; }

        public string? UserName { get; set; }

        public int CurrencyId { get; set; }

        public int SiteId { get; set; }

        public decimal LockedAmount { get; set; }

        public decimal BonusAmount { get; set; }

        public DateTime ExpiryTime { get; set; }

        public string? BonusInfo { get; set; }

        public string? Memo { get; set; }

        public DateTime CreateTime { get; set; }

        public int Status { get; set; }
    }
}
