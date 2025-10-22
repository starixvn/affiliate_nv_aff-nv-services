using System;

namespace Affiliate.N88.Models
{
    public class RebateMLogModel
    {
        public int Id { get; set; }

        public int CustId { get; set; }

        public byte CurrencyId { get; set; }

        public byte RebateType { get; set; }

        public string? UserName { get; set; }

        public int SiteId { get; set; }

        public decimal RedeemAmount { get; set; }

        public string? BonusCode { get; set; }

        public string? Memo { get; set; }

        public long RelId { get; set; }

        public DateTime CreateTime { get; set; }
    }
}
