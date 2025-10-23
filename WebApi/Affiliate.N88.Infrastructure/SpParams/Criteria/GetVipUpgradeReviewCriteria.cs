using Fanex.Data.Repository;

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
