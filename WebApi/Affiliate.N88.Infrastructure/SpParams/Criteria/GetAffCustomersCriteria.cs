using Fanex.Data;
using Fanex.Data.Repository;
using System;

namespace Affiliate.N88.SpParams.Criteria
{
    public class GetAffCustomersCriteria : CriteriaBase
    {
        private const string SP_NAME = "Aff_CustomerSel";

        private string? Prefix { get; }

        public GetAffCustomersCriteria(DateTime fromDate, DateTime toDate, int pageIndex, int pageSize, string? prefix = null)
        {
            Prefix = prefix;
            FromDate = fromDate;
            ToDate = toDate;
            PageIndex = pageIndex;
            PageSize = pageSize;
        }

        public override string GetSettingKey() => string.IsNullOrWhiteSpace(Prefix) ? SP_NAME: $"{Prefix}_{SP_NAME}";

        public override bool IsValid() => true;

        [SpParam(SqlParamName = "_FromDate")]
        public DateTime FromDate { get; }

        [SpParam(SqlParamName = "_ToDate")]
        public DateTime ToDate { get; set; }

        [SpParam(SqlParamName = "_PageIndex")]
        public int PageIndex { get; set; }

        [SpParam(SqlParamName = "_PageSize")]
        public int PageSize { get; }
    }
}