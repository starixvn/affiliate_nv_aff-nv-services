using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Affiliate.N88.Models
{
    public class AdjustmentInfo
    {
        public int AdjustId { get; set; }

        public string? TransId { get; set; }

        public int AdjustType { get; set; }

        public int CustId { get; set; }

        public string? UserName { get; set; }

        public int CurrencyId { get; set; }

        public int SiteId { get; set; }

        public string? AdjustIdInfo { get; set; }

        public decimal Amount { get; set; }

        public DateTime CreateTime { get; set; }

        public bool FirstDeposit { get; set; }

        public string? RefId { get; set; }

        public string? Memo { get; set; }

        public string? Comment { get; set; }

        public int Status { get; set; }
    }
}
