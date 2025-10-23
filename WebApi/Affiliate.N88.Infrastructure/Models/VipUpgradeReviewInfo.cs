namespace Affiliate.N88.Models
{
    public class VipUpgradeReviewInfo
    {
        public int CustId { get; set; }

        public string? CalculateMonth { get; set; }

        public string? UserName { get; set; }

        public int SiteId { get; set; }

        public int CurrentVipLevel { get; set; }

        public int UpgradeVipLevel { get; set; }

        public int CurrencyId { get; set; }

        public decimal DepositAmount { get; set; }

        public decimal UpgradeAmount { get; set; }

        public int Status { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime LastUpdateTime { get; set; }

        public DateTime SettleTime { get; set; }

        public string? LastUpdateUser { get; set; }
    }
}
