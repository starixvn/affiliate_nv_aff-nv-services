using System;

namespace Affiliate.N88.Models
{
    public class CustBonusRolloverModel
    { 
        public int BonusId { get; set; }

        public int PromoCategoryId { get; set; }

        public int TpId { get; set; }

        public int CustId { get; set; }

        public int SiteId { get; set; }

        public decimal RollOver { get; set; }

        public decimal TurnOver { get; set; }

        public int Status { get; set; }

        public DateTime CreateTime { get; set; }

        public DateTime? UpdateTime { get; set; }
    }
}
