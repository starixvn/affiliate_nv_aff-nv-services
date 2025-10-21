using Affiliate.N88.Helpers;
using Affiliate.N88.Jobs.Apis;
using Affiliate.N88.Jobs.Configurations;
using Fanex.Data;
using Fanex.Data.Repository;
using Fanex.Data.SqlServer;
using Fanex.Logging;
using Fanex.Logging.Sentry.RavenClient;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ONEbook.UM.AspNetCore;
using ONEbook.UM.Configurations;
using ONEbook.UM.Helpers;
using ONEbook.UM.Services;
using ONEbook.UM.Transport;
using Refit;
using SharpRaven;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Affiliate.N88.Jobs.Middlewares
{
    public static class ServicesMiddleware
    {
        private static AppSettings appSettings;

        public static void RegisterServices(this IServiceCollection services, IConfiguration configuration, AppSettings appSettings)
        {
            ServicesMiddleware.appSettings = appSettings;
            services.AddSingleton<IDynamicRepository, DynamicRepository>();

            InitApi(services);
            InitLogging(services);
            InitFanexData(configuration);
        }

        public static void InitLogging(IServiceCollection services)
        {
            LogManager
                    .SetDefaultLogCategory("Affiliate.N88.DataApi")
                    .Use(new SentryLogging(new SentryEngineOptions
                    {
                        Dsn = new Dsn(appSettings.SentryDsn)
                    }));

            services.AddSingleton(Logger.Log);
        }

        public static void InitFanexData(IConfiguration configuration)
        {
            var listConnectionName = configuration.GetSection("ConnectionStrings").GetChildren().Select(_ => _.Key);
            var connectionStrings = new Dictionary<string, ConnectionConfiguration>(listConnectionName
                .Select(connectionName => new KeyValuePair<string, ConnectionConfiguration>(connectionName,
                    new ConnectionConfiguration(connectionName, configuration.GetConnectionString(connectionName)))
                ));

            DbSettingProviderManager
                .StartNewSession()
                .WithSqlServer(configuration)
                .Use(connectionStrings)
                .Run();
        }

        private static void InitApi(IServiceCollection services)
        {
            Func<HttpClient> CreateHttpClient = () =>
            {
                var httpCLient = HttpClientFactory.Create(
                      new AuthenticatedHttpClientHandler(appSettings.AuthenticationKey)
                      {
                          Proxy = appSettings.ProxySetting != null && appSettings.ProxySetting.IsUseProxy
                              ? new WebProxy(new Uri(appSettings.ProxySetting.ProxyUrl))
                              : null,
                          AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
                      });
                httpCLient.BaseAddress = new Uri(appSettings.LicenseeDataAPI);
                httpCLient.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip"));
                return httpCLient;
            };

            services.AddSingleton(RestService.For<ILicenseeApi>(CreateHttpClient()));
            services.AddSingleton(RestService.For<IPromotionApi>(CreateHttpClient()));
            services.AddSingleton(RestService.For<IProduct9WApi>(CreateHttpClient()));
        }

        public static void UsePing(this IApplicationBuilder app)
        {
            app.Use(async (context, next) =>
            {
                if (context.Request.Path.StartsWithSegments("/ping"))
                {
                    return;
                }
                await next();
            });
        }

        public static void AddUnderMaintenance(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton(new UMClientConfiguration().Initialize(configuration));
            services.AddSingleton<IUMCacheService, UMCacheService>();
            services.AddSingleton<IUMRestClient, UMRestClient>();
            services.AddSingleton<IHttpContextHelper, ContextInMemoryHelper>();
            services.AddSingleton<IMaintenanceService, MaintenanceService>();
            services.AddMemoryCache();
        }

        internal class ContextInMemoryHelper : IHttpContextHelper
        {
            private readonly TimeSpan ByPassUMCacheTime = TimeSpan.FromMinutes(30);
            private readonly IHttpContextAccessor httpContextAccessor;
            private readonly IMemoryCache cache;

            public ContextInMemoryHelper(IHttpContextAccessor httpContextAccessor, IMemoryCache cache)
            {
                this.httpContextAccessor = httpContextAccessor;
                this.cache = cache;
            }

            public string GetSession(string key)
                => cache.TryGetValue(BuildKey(key), out string value) ? value : string.Empty;

            public string GetValueFromQueryString(string queryKey)
                => httpContextAccessor.HttpContext?.Request?.Query[queryKey].ToString();

            public void SetSession(string key, string value)
                => cache.Set(BuildKey(key), value, ByPassUMCacheTime);

            private string BuildKey(string key)
                => $"{key}:{httpContextAccessor.HttpContext?.Connection?.RemoteIpAddress?.ToString()}";
        }
    }
}