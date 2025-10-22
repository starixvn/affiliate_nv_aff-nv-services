using Fanex.Data;
using Fanex.Data.Repository;

namespace Affiliate.N88.SpParams.Commands
{
    public class InsertAffiliateInfoCommand : NonQueryCommand
    {
        [SpParam(SqlParamName = "_CustId")]
        public int CustId { get; set; }

        [SpParam(SqlParamName = "_SiteId")]
        public int SiteId { get; set; }

        [SpParam(SqlParamName = "_AffiliateId")]
        public int AffiliateId { get; set; }

        [SpParam(SqlParamName = "_AffiliateName")]
        public string? AffiliateName { get; set; }

        [SpParam(SqlParamName = "_ParentAffiliate")]
        public string? ParentAffiliate { get; set; }

        [SpParam(SqlParamName = "_AffiliateCode")]
        public string? AffiliateCode { get; set; }

        [SpParam(SqlParamName = "_UserName")]
        public string? UserName { get; set; }

        [SpParam(SqlParamName = "_FirstName")]
        public string? FirstName { get; set; }

        [SpParam(SqlParamName = "_LastName")]
        public string? LastName { get; set; }

        [SpParam(SqlParamName = "_ContactNumber")]
        public string? ContactNumber { get; set; }

        [SpParam(SqlParamName = "_Email")]
        public string? Email { get; set; }

        [SpParam(SqlParamName = "_Status")]
        public string? Status { get; set; }

        public override string GetSettingKey() => "Affiliate_AffiliateInfo_Set";

        public override bool IsValid() => true;
    }
}

