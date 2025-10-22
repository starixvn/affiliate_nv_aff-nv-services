using System;

namespace Affiliate.N88.Models
{
    public class TpReport9W
    {
        public long SQ { get; set; }

        public string? UserId { get; set; }

        public string? AgentId { get; set; }

        public string? EventId { get; set; }

        public string? MarketId { get; set; }

        public DateTime SettledDate { get; set; }

        public DateTime? ResettledDate { get; set; }

        public decimal ProfitLoss { get; set; }

        public decimal MarketTotal { get; set; }

        public decimal Commission { get; set; }

        public decimal AgentCommission { get; set; }

        public decimal StakeTotal { get; set; }

        public DateTime? CreateTime { get; set; }

        public DateTime? UpdateTime { get; set; }

        public int CustId { get; set; }
    }
}
