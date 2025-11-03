using Affiliate.N88.Jobs.Configurations;
using Affiliate.N88.Jobs.Middlewares;
using Fanex.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;

namespace Affiliate.N88.Jobs.Extensions
{
    public static class HealthCheckExtensions
    {
        public static IHealthChecksBuilder AddSiteHealthChecks(this IServiceCollection services, IConfiguration configuration)
        {
            var redisSetting = configuration.GetSection("Redis").Get<RedisSetting>();

            return services.AddHealthChecks()
                .AddCheck<WhoAmIHealthCheck>("WhoAmI")
                .AddRedis($"{redisSetting.Server}:{redisSetting.Port},password={redisSetting.Pass}", "Redis");
                //.AddMultiSqlServer(configuration);
        }

        public static void UseSiteHealthChecks(this IApplicationBuilder app, IConfiguration configuration)
        {
            var accessKey = configuration["HealthCheck:AccessKey"];
            app.UseWhen(context =>
                string.IsNullOrEmpty(accessKey) || (context.Request.Query.TryGetValue("AccessKey", out StringValues value) && accessKey.Equals(value)),
                     builder => builder.UseHealthChecks("/WhoAmI", options: new HealthCheckOptions
                     {
                         AllowCachingResponses = false,
                         Predicate = _ => _.Name.Equals("WhoAmI"),
                         ResponseWriter = CustomResponseWriter
                     }).UseHealthChecks("/healthz", options: new HealthCheckOptions
                     {
                         AllowCachingResponses = false,
                         Predicate = _ => !_.Name.Equals("WhoAmI"),
                         ResponseWriter = CustomResponseWriter
                     }));
        }

        private static async Task CustomResponseWriter(HttpContext context, HealthReport report)
        {
            var result = JsonConvert.SerializeObject(
                new
                {
                    status = report.Status.ToString(),
                    monitors = report.Entries.Select(e => new { name = e.Key, status = Enum.GetName(typeof(HealthStatus), e.Value.Status), values = e.Value.Data })
                }, Formatting.Indented);

            context.Response.ContentType = MediaTypeNames.Application.Json;
            await context.Response.WriteAsync(result);
        }
    }
}
