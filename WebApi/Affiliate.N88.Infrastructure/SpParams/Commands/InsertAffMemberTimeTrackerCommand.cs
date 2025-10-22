using Affiliate.N88.Core;
using System;

namespace Affiliate.N88.SpParams.Commands
{
    public class InsertAffMemberTimeTrackerCommand : IdenticalSuffixNonQueryCommand
    {
        public DateTime LastImportedTime { get; set; }

        public InsertAffMemberTimeTrackerCommand(IPrefixProvider prefixProvider, string spName) : base(prefixProvider)
        {
            SName = spName;
        }

        public string SName { get; }

        protected override string SuffixSettingKey => "Task_Aff_MemberInfo_ImportedTime";

        public override bool IsValid() => true;

        public int ErrCode { get; set; }

        public string? ErrMsg { get; set; }
    }
}