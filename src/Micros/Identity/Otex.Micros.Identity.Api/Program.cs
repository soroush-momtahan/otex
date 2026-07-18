using System.Globalization;
using System.Text;
using Duende.IdentityServer.Licensing;
using Microsoft.AspNetCore.HttpOverrides;
using Otex.Micros.Identity.Api;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console(formatProvider: CultureInfo.InvariantCulture)
    .CreateBootstrapLogger();

Log.Information("Starting up");

try
{
    var builder = WebApplication.CreateBuilder(args);
    
    builder.Services.Configure<ForwardedHeadersOptions>(options =>
    {
        options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedPrefix | ForwardedHeaders.XForwardedProto | ForwardedHeaders.XForwardedHost;
        // در محیط توسعه، شبکه‌های شناخته شده را خالی می‌گذاریم تا همه پروکسی‌ها را قبول کند
        options.ForwardLimit = null; 
        options.KnownNetworks.Clear();
        options.KnownProxies.Clear();
    });

    var app = builder
        .ConfigureServices()
        .ConfigurePipeline();
    
    // app.UsePathBase("/api/identity");
    // app.UseForwardedHeaders();
    
//      app.UseForwardedHeaders(); 
// //
// // // این خط بسیار مهم است: اگر هدر Prefix وجود داشت، Base Path اپلیکیشن را تغییر می‌دهد
//      app.Use((context, next) =>
//      {
//          if (context.Request.Headers.TryGetValue("X-Forwarded-Prefix", out var prefix))
//          {
//              context.Request.PathBase = prefix.ToString();
//          }
//          return next();
//      });

    // this seeding is only for the template to bootstrap the DB and users.
    // in production you will likely want a different approach.
    if (args.Contains("/seed"))
    {
        Log.Information("Seeding database...");
        SeedData.EnsureSeedData(app);
        Log.Information("Done seeding database. Exiting.");
        return;
    }

    if (app.Environment.IsDevelopment())
    {
        _ = app.Lifetime.ApplicationStopping.Register(() =>
        {
            var usage = app.Services.GetRequiredService<LicenseUsageSummary>();
            Console.Write(Summary(usage));
        });
    }

    app.Run();
}
catch (Exception ex) when (ex is not HostAbortedException)
{
    Log.Fatal(ex, "Unhandled exception");
}
finally
{
    Log.Information("Shut down complete");
    Log.CloseAndFlush();
}

static string Summary(LicenseUsageSummary usage)
{
    var sb = new StringBuilder();
    _ = sb.AppendLine("IdentityServer Usage Summary:");
    _ = sb.AppendLine(CultureInfo.InvariantCulture, $"  License: {string.Join(", ", usage.EntitledSkus)}");
    var features = usage.FeaturesUsed.Count > 0 ? string.Join(", ", usage.FeaturesUsed) : "None";
    _ = sb.AppendLine(CultureInfo.InvariantCulture, $"  Business and Enterprise Edition Features Used: {features}");
    _ = sb.AppendLine(CultureInfo.InvariantCulture, $"  {usage.ClientsUsed.Count} Client Id(s) Used");
    _ = sb.AppendLine(CultureInfo.InvariantCulture, $"  {usage.IssuersUsed.Count} Issuer(s) Used");

    return sb.ToString();
}