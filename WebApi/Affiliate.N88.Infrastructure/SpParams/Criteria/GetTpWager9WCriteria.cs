using Fanex.Data;
using Fanex.Data.Repository;

namespace Affiliate.N88.SpParams.Criteria
{
    public class GetTpWager9WCriteria : CriteriaBase
    {
        [SpParam(SqlParamName = "_CreateTime")]
        public DateTime CreateTime { get; set; }

        [SpParam(SqlParamName = "_FromDate")]
        public DateTime FromDate { get; set; }

        [SpParam(SqlParamName = "_ToDate")]
        public DateTime ToDate { get; set; }

        [SpParam(SqlParamName = "_BatchCount")]
        public int BatchCount { get; set; }

        public override string GetSettingKey()
            => "API_TpWager9W_Get";

        public override bool IsValid()
            => true;
    }
}
