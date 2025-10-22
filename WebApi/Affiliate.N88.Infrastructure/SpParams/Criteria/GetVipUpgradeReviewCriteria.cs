using Fanex.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Affiliate.N88.SpParams.Criteria
{
    public class GetVipUpgradeReviewCriteria : CriteriaBase
    {
        public string? MonthDate { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public override string GetSettingKey()
            => "API_VipUpgradeReview_Get";

        public override bool IsValid()
            => true;
    }
}
