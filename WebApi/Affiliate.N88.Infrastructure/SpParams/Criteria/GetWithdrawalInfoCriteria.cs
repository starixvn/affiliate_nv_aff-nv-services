using Fanex.Data.Repository;
using System;

namespace Affiliate.N88.SpParams.Criteria
{
    public class GetWithdrawalInfoCriteria : CriteriaBase
    {
        public DateTime UpdateTime { get; }

        public int BatchCount { get; }

        public GetWithdrawalInfoCriteria(DateTime updateTime, int batchCount)
        {
            UpdateTime = updateTime;
            BatchCount = batchCount;
        }

        public override string GetSettingKey()
            => "API_WithdrawalInfo_Get";

        public override bool IsValid()
            => true;
    }
}
