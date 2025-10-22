using Fanex.Data.Repository;
using System;

namespace Affiliate.N88.SpParams.Criteria
{
    public class GetCustVerificationRewardCriteria : CriteriaBase
    {
        public DateTime LastRewardTime { get; set; }

        public string? ConditionField { get; set; }

        public int BatchCount { get; set; }

        public GetCustVerificationRewardCriteria(DateTime lastRewardTime, string conditionField, int batchCount)
        {
            LastRewardTime = lastRewardTime;
            ConditionField = conditionField;
            BatchCount = batchCount;
        }

        public override string GetSettingKey() => "API_CustVerificationReward_Get";

        public override bool IsValid() => true;
    }
}
