using Fanex.Data;
using Fanex.Data.Repository;
using System;

namespace Affiliate.N88.SpParams.Criteria
{
    public class GetTpReport9WCriteria : CriteriaBase
    {
        [SpParam(SqlParamName = "_SQ")]
        public long SQ { get; set; }

        [SpParam(SqlParamName = "_FromDate")]
        public DateTime FromDate { get; set; }

        [SpParam(SqlParamName = "_ToDate")]
        public DateTime ToDate { get; set; }

        [SpParam(SqlParamName = "_BatchCount")]
        public int BatchCount { get; set; }

        public override string GetSettingKey()
            => "API_TpReport9W_Get";

        public override bool IsValid()
            => true;
    }
}
