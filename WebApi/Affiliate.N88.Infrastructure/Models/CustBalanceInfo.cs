using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Affiliate.N88.Models
{
    public class CustBalanceInfo
    {
        public int CustId { get; set; }

        public string? UserName { get; set; }

        public int CurrencyId { get; set; }

        public int  Status { get; set; }

        public int ActStatus { get; set; }

        public decimal Balance { get; set; }
    }
}
