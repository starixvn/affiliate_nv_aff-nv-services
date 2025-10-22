using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Affiliate.N88.Models
{
    public class CustVerificationReward
    {
        public int CustId { get; set; }

        public int SiteId { get; set; }

        public decimal? PhoneReward { get; set; }

        public DateTime? PhoneRewardTime { get; set; }

        public decimal? EmailReward { get; set; }

        public DateTime? EmailRewardTime { get; set; }

        public int CurrencyId { get; set; }
    }
}
