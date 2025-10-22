using Affiliate.N88.WebApi.Health;
using Affiliate.N88.WebApi.Models.HelthCheck;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Affiliate.N88.WebApi.Controllers
{
    [Route("api/health")]
    [ApiController]
    public class HealthController : ControllerBase
    {
        private IEnumerable<IHealthCheckProvider> HealthCheckProviders { get; }

        public HealthController(IEnumerable<IHealthCheckProvider> healthCheckProviders)
        {
            HealthCheckProviders = healthCheckProviders;
        }

        [HttpGet("Index")]
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var result = new HealthCheckResult 
            {
                MachineName = Environment.MachineName
            };
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            foreach (var provider in HealthCheckProviders)
            {
                result.HealthChecks.Add(await provider.GetHealthCheckAsync());
            }
            stopwatch.Stop();

            result.TimeTakenInSeconds = stopwatch.Elapsed.TotalSeconds;
            return Ok(result);
        }
    }
}