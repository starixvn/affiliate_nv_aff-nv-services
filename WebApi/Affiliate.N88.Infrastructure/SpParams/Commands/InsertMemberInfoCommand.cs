using Affiliate.N88.Core;
using Affiliate.N88.SpParams.Commands;
using Fanex.Data;

namespace Affiliate.N88.DTO
{
    public class InsertMemberInfoCommand : IdenticalSuffixNonQueryCommand
    {
        public InsertMemberInfoCommand(IPrefixProvider prefixProvider) : base(prefixProvider)
        { }

        [SpParam(Ignored = true)]
        public string? SettingPrefix { get; set; }

        [SpParam(Ignored = true)]
        public string? SiteName { get; set; }

        public string? XmlData { get; set; }

        public int ErrCode { get; set; }

        public string? ErrMsg { get; set; }

        protected override string SuffixSettingKey => "Task_Aff_MemberInfo_Import";

        public override bool IsValid() => true;
    }
}
