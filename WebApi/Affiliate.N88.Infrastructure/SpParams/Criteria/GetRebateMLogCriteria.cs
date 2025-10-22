using Fanex.Data.Repository;
using System;

namespace Affiliate.N88.SpParams.Criteria
{
    public class GetRebateMLogCriteria : CriteriaBase
    {
        public GetRebateMLogCriteria(DateTime fromDate, DateTime toDate)
        {
            FromDate = fromDate;
            ToDate = toDate;
        }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public override string GetSettingKey() => "API_RebateMLog_Get";

        public override bool IsValid() => true;
    }
}
