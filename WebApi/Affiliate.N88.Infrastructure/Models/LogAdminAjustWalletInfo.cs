using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Affiliate.N88.Models
{
    public class LogAdminAjustWalletInfo
    {
        public int TransferId { get; set; }

        public string? TransId { get; set; }

        public string? ThirdId { get; set; }

        public int TpId { get; set; }

        public int CustId { get; set; }

        public string? UserName { get; set; }

        public int SiteId { get; set; }

        public int Direction { get; set; }

        public decimal Amount { get; set; }

        public string? Memo { get; set; }

        public int Status { get; set; }

        public DateTime CreateTime { get; set; }

        public string? Operator { get; set; }

        public string? Remarks { get; set; }

        public int CurrencyId { get; set; }
    }
}
