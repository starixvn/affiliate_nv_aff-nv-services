using System;

namespace Affiliate.N88.Models
{
    public class CustRegModel
    {
        public int CustId { get; set; }

        public int? SiteId { get; set; }

        public string? SignUpUrl { get; set; }

        public DateTimeOffset RegTime { get; set; }
    }
}
