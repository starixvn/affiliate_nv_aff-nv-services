using System;

namespace Affiliate.N88.Models
{
    public class CustomerInfo
    {
        public int CustId { get; set; }

        public int? SiteId { get; set; }

        public string? UserName { get; set; }

        public int? CurrencyId { get; set; }

        public string? UserLang { get; set; }

        public int? ActStatus { get; set; }

        public int? AgentId { get; set; }

        public string? AllAgentId { get; set; }

        public int? UserLevel { get; set; }

        public DateTime? RegTime { get; set; }

        public bool? IsTest { get; set; }

        public int? Status { get; set; }

        public int MemberType { get; set; }
    }
}
