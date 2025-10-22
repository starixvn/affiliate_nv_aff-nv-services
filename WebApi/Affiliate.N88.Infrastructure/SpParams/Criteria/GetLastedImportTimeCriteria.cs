using Affiliate.N88.Core;

namespace Affiliate.N88.SpParams.Criteria
{
    public class GetLastedImportTimeCriteria : IdenticalSurfixCriteriaBase
    {
        public GetLastedImportTimeCriteria(IPrefixProvider? prefixProvider = null) 
            : base(prefixProvider) {}

        protected override string SuffixSettingKey => "Task_API_TimeTracker_Sel";

        public override bool IsValid() => true;

        public string? DataSource { get; set; }
    }
}
