using Affiliate.N88.Core;
using Fanex.Data.Repository;

namespace Affiliate.N88.SpParams.Commands
{
    public class UpdateDataTrackerCommand : IdenticalSuffixNonQueryCommand, INonQueryCommand
    {
        public UpdateDataTrackerCommand(IPrefixProvider prefixProvider) : base(prefixProvider)
        { }

        public int PushTrackerId { get; set; }

        public int StatusId { get; set; }

        public int ErrCode { get; set; }

        public string? ErrMsg { get; set; }

        protected override string SuffixSettingKey => "Task_API_PushDataTracker_Upd";

        public override bool IsValid()
            => true;
    }
}
