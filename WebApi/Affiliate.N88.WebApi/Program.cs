using Affiliate.N88.WebApi.Filters;
using Affiliate.N88.WebApi.Health;
using Affiliate.N88.WebApi.Middleware;
using Affiliate.N88.WebApi.Repository;
using Fanex.Data;
using Fanex.Data.MySql;
using Fanex.Data.Repository;
using Fanex.Logging;
using Fanex.Logging.Sentry.RavenClient;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using SharpRaven;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMemoryCache();

// Global filters & JSON
var isTest = builder.Configuration.GetValue<bool>("IsTest");
var authenticationKey = builder.Configuration["AuthenticationKey"];

if (!isTest)
{
    builder.Services.AddScoped<HmacAuthorizationFilter>(provider =>
        new HmacAuthorizationFilter(authenticationKey ?? string.Empty, provider.GetRequiredService<IMemoryCache>()));
}

builder.Services.AddControllers(options =>
{
    if (!isTest)
    {
        options.Filters.AddService(typeof(HmacAuthorizationFilter));
    }
})
.AddNewtonsoftJson(options =>
{
    options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Serialize;
});

builder.Services.Configure<JsonOptions>(options =>
{
    options.JsonSerializerOptions.PropertyNamingPolicy = null; // Use PascalCase
});

// Register repositories with built-in DI container
builder.Services.AddSingleton<IDynamicRepository, DynamicRepository>();
builder.Services.AddScoped<IPromotionRepository, PromotionRepository>();

// Register health check providers
RegisterHealthCheck(builder.Services);

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Use Global Exception Middleware
app.UseMiddleware<GlobalExceptionMiddleware>();

app.UseAuthorization();

// Default root endpoint
app.MapGet("/", () => "API running");

// Configure routing
app.MapControllers();

// Configure custom routes
app.MapControllerRoute(
    name: "FakeApi",
    pattern: "api/Account/{action}/{id?}",
    defaults: new { controller = "Fake" }
);

app.MapControllerRoute(
    name: "DefaultApi",
    pattern: "api/{controller}/{action}/{id?}"
);

// Initialize logging and data access
InitializeLogging(app.Configuration);
InitializeFanexData(app.Configuration);

await app.RunAsync();

static void InitializeLogging(IConfiguration configuration)
{
    var env = configuration["Sentry:Environment"];
    var backEndDsn = configuration["Sentry:Dsn"];
    var category = configuration["Sentry:LogCategory"];

    if (!string.IsNullOrEmpty(backEndDsn))
    {
        LogManager.SetDefaultLogCategory(category).Use(new List<ILogEngine>
    {
        new SentryLogging(new SentryEngineOptions
        {
            Dsn = new Dsn(backEndDsn),
            Environment = env
        })
    });

    }
}

static void InitializeFanexData(IConfiguration configuration)
{
    DbSettingProviderManager.StartNewSession()
                .WithMySql(configuration)
                .UseDefaultDbSettingProvider("App_Data/SpNames.xml", true)
                .Run();
}

// Extension method for health check registration
static void RegisterHealthCheck(IServiceCollection services)
{
    const int masterSortOrder = 20;
    const int slaveSortOrder = 21;
    
    services.AddScoped<IHealthCheckProvider>(provider =>
    {
        var repo = provider.GetRequiredService<IDynamicRepository>();
        return new DatabaseHealthCheckProvider(repo, Affiliate.N88.WebApi.Infastructure.Enums.DatabaseServerType.Master, masterSortOrder);
    });
    
    services.AddScoped<IHealthCheckProvider>(provider =>
    {
        var repo = provider.GetRequiredService<IDynamicRepository>();
        return new DatabaseHealthCheckProvider(repo, Affiliate.N88.WebApi.Infastructure.Enums.DatabaseServerType.Slave, slaveSortOrder);
    });
}
