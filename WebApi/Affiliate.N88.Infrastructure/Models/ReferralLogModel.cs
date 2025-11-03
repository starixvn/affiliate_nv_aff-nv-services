namespace Affiliate.N88.Models
{
    public class ReferralLogModel
    {
        public int SQ { get; set; }

        public int CustId { get; set; }

        public int CurrencyId { get; set; }

        public string? UserName { get; set; }

        public int SiteId { get; set; }

        public decimal ReferralFreeCredit { get; set; }

        public int AgentId { get; set; }

        public decimal ReferralAmount { get; set; }

        public string? Memo { get; set; }

        public long RelId { get; set; }

        public DateTime CreateTime { get; set; }

        public int RewardStatus { get; set; }
    }
}