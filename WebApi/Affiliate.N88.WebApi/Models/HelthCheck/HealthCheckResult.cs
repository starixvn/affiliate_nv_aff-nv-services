using Affiliate.N88.WebApi.Infastructure.Enums;
using System.Collections.Generic;
using System.Linq;

namespace Affiliate.N88.WebApi.Models.HelthCheck
{
    public class HealthCheckResult
    {
        /// <summary>
        /// Indicates if at least one of the connected health checks is not considered healthy.
        /// </summary>
        public bool HasFailures
        {
            get { return HealthChecks.Any(x => x.HealthState != HealthState.Healthy); }
        }

        /// <summary>
        /// Gets or sets the current machine name.
        /// </summary>
        public string? MachineName { get; set; }

        /// <summary>
        /// Gets or sets the total time in seconds it took to test all connected health check providers.
        /// </summary>
        public double TimeTakenInSeconds { get; set; }

        /// <summary>
        /// Gets the information of the connected health check providers.
        /// </summary>
        public List<HealthCheckItemResult> HealthChecks { get; } = new List<HealthCheckItemResult>();
    }
}