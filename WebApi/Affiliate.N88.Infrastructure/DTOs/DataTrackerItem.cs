
using Affiliate.N88.Core;
using Fanex.Data;

namespace Affiliate.N88.DTOs
{
    public class DataTrackerItem
    {
        [SpParam(Ignored = true)]
        public IPrefixProvider? PrefixProvider { get; set; }

        public int PushTrackerId { get; set; }

        public int StatusId { get; set; }

        public int PushDataType { get; set; }

        public string? PushDataInfo { get; set; }
    }
}
