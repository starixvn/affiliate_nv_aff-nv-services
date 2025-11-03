namespace Affiliate.N88.Models
{
    public class PromotionSpecificModel
    {
        public string? BonusCode { get; set; }

        public byte CurrencyId { get; set; }

        public int SiteId { get; set; }

        public int Usage { get; set; }

        public int BeenUsed { get; set; }

        public int RollOver { get; set; }

        public decimal Bonus { get; set; }

        public byte BonusType { get; set; }

        public decimal RequiredAmount { get; set; }

        public string? LockedType { get; set; }

        public decimal MaxBonus { get; set; }

        public byte Status { get; set; }

        public string? Details { get; set; }

        public DateTime CreateTime { get; set; }

        public DateTime? UpdateTime { get; set; }

        public DateTime? LastClaimTime { get; set; }

        public decimal MinPayout { get; set; }

        public DateTime? SettleTime { get; set; }

        public int AffId { get; set; }

        public int VN_AffId { get; set; }

        public string? VN_AffIds { get; set; }

        public string? VN_AffCodes { get; set; }
    }
}
