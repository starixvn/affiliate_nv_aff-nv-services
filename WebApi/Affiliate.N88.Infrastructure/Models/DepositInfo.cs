using System;

namespace Affiliate.N88.Models
{
    public class DepositInfo
    {
        public int DepositId { get; set; }

        public string? TransId { get; set; }

        public int? DepositType { get; set; }

        public int? CustId { get; set; }

        public string? UserName { get; set; }

        public int? CurrencyId { get; set; }

        public int? SiteId { get; set; }

        public string? DespositInfo { get; set; }

        public decimal? Amount { get; set; }

        public decimal? ActualDeposit { get; set; }

        public DateTime? CreateTime { get; set; }

        public DateTime? TPCreateTime { get; set; }

        public DateTime? CallbackTime { get; set; }

        public int? FirstDeposit { get; set; }

        public string? ThirdId { get; set; }

        public string? RefId { get; set; }

        public DateTime? DepositTime { get; set; }

        public string? Memo { get; set; }

        public string? PostScript { get; set; }

        public string? Comment { get; set; }

        public int? Status { get; set; }

        public int? Flag { get; set; }

        public string? Operator { get; set; }

        public DateTime? UpdateTime { get; set; }
    }
}
