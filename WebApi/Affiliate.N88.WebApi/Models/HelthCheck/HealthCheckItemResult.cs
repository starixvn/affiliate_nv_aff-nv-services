using Affiliate.N88.WebApi.Infastructure.Enums;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Affiliate.N88.WebApi.Models.HelthCheck
{
    public class HealthCheckItemResult
    {
        public HealthCheckItemResult(string resourceName, int sortOrder, string friendlyName, string? description = null)
        {
            ResourceName = resourceName;
            SortOrder = sortOrder;
            FriendlyName = friendlyName;
            Description = description;
        }

        public string? Description { get; set; }

        public string FriendlyName { get; }

        [JsonIgnore]
        public virtual HealthState HealthState  { get; set; }

        public IList<MessageContainer> Messages => new List<MessageContainer>();

        public string ResourceName { get; }

        [JsonIgnore]
        public int SortOrder { get; }

        public virtual string State => HealthState.ToString();
    }
}