using System;

namespace Affiliate.N88.WebApi.DTO
{
    public class Promotion
    {
        public string? BonusCode { get; set; }

        public int SiteId { get; set; }

        public string? Description { get; set; }

        public int PromotionType { get; set; }

        public int CategoryId { get; set; }

        public int? TpId { get; set; }

        public long PeriodFrom { get; set; }

        public long PeriodTo { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public int ExpiryDays { get; set; }

        public int Sort { get; set; }

        public string? CreateTime { get; set; }

        public int Status { get; set; }

        public DateTime? LastUpdateTime { get; set; }

        public string? LastUpdateUser { get; set; }

        public int? WalletType { get; set; }

        public int? RedeemHours { get; set; }

        public int? PeriodType { get; set; }

        public int? PeriodDay { get; set; }

        public string? SpecialCode { get; set; }

        public bool IsSpecial { get; set; }
    }
}