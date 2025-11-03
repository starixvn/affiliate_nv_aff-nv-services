using Affiliate.N88.Jobs.Configurations;
using Affiliate.N88.Jobs.Extensions;
using Affiliate.N88.Jobs.Middlewares;
using Fanex.Logging;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System;
using Microsoft.Extensions.Hosting;
using Hangfire;

namespace Affiliate.N88.Jobs
{
    public class Startup
    {
        private AppSettings appSettings;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            appSettings = new AppSettings();
            Configuration.GetSection("AppSettings").Bind(appSettings);

            services.AddControllers();

            services.AddHttpContextAccessor();
            services.AddHttpClient();

            services.RegisterServices(Configuration, appSettings);
            services.AddUnderMaintenance(Configuration);
            services.AddSiteHealthChecks(Configuration);
            services.RegisterHangfire(Configuration);
            const int HttpsPort = 443;
            services.AddHttpsRedirection(options =>
            {
                options.HttpsPort = HttpsPort;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UsePing();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseHsts();
                app.UseWhen(
                    context => !Dns.GetHostName().Contains(context.Request.Host.Host, StringComparison.OrdinalIgnoreCase),
                    builder => builder.UseHttpsRedirection());
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }
            app.UseMiddleware<ErrorHandler>();
            app.UseSiteHealthChecks(Configuration);
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            app.UseRouting();

            GlobalJobFilters.Filters.Add(new AutomaticRetryAttribute { Attempts = 0 });
            app.UseHangfire(appSettings);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Hello, Affiliate NV CronJobs are running!");
                });
            });

            Logger.Log.Info("Affiliate N88 Job started!");
        }
    }
}