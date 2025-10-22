
using System;

namespace Affiliate.N88.DTO
{
    public class AffCustomerInfo
    {
        public int CustId { get; set; }

        public int SiteId { get; set; }

        public string? SiteName { get; set; }

        public int? AffId { get; set; }

        public string? UserName { get; set; }

        public string? Userpwd { get; set; }

        public int? CurrencyId { get; set; }

        public string? UserLang { get; set; }

        public byte? ActStatus { get; set; }

        public byte? UserLevel { get; set; }

        public DateTime? LastLoginTime { get; set; }

        public DateTime? RegTime { get; set; }

        public DateTime? FirstDepositTime { get; set; }

        public bool? IsTest { get; set; }

        public bool? IsKyc { get; set; }

        public bool? IsRisk { get; set; }

        public int? Status { get; set; }

        public string? AffCode { get; set; }
    }
}