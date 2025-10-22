using Affiliate.N88.Core;
using System;

namespace Affiliate.N88.SpParams.Commands
{
    public class InsertImportTrackerCommand : IdenticalSuffixNonQueryCommand
    {
        public InsertImportTrackerCommand(IPrefixProvider? prefixProvider = null) : base(prefixProvider) { }
        protected override string SuffixSettingKey => "Task_API_TimeTracker_Import";

        public override bool IsValid() => true;

        public string? DataSource { get; set; }

        public DateTime CalledTime { get; set; }

        public bool IsReset { get; set; }

        public string? KeyValue { get; set; }

        public int ErrCode { get; set; }

        public string? ErrMsg { get; set; }
    }
}
