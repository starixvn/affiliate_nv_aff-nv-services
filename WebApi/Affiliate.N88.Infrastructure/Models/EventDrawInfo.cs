namespace Affiliate.N88.Models
{
    public class EventDrawInfo
    {
        public int EventId { get; set; }

        public string? EventName { get; set; }

        public int SiteId { get; set; }

        public int EventType { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public string? TimeZone { get; set; }

        public DateTime? ResetDate { get; set; }

        public string? ConditionSP { get; set; }

        public DateTime CreateDate { 
            get {
                return CreateTime;
            } 
        }

        public DateTime CreateTime { get; set; }

        public DateTime UpdateTime { get; set; }
    }
}
