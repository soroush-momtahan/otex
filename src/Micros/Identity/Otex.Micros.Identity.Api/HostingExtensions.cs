using System.Globalization;
using Duende.IdentityServer;
using Duende.IdentityServer.EntityFramework.DbContexts;
using Duende.IdentityServer.EntityFramework.Mappers;
using Otex.Micros.Identity.Api.Data;
using Otex.Micros.Identity.Api.Models;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Serilog.Filters;

namespace Otex.Micros.Identity.Api;

internal static class HostingExtensions
{
    public static WebApplicationBuilder ConfigureLogging(this WebApplicationBuilder builder)
    {
        // Write most logs to the console but diagnostic data to a file.
        // See https://duende.link/diagnostics
        _ = builder.Services.AddSerilog(lc =>
        {
            _ = lc.WriteTo.Logger(consoleLogger =>
            {
                _ = consoleLogger.WriteTo.Console(
                    outputTemplate:
                    "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}",
                    formatProvider: CultureInfo.InvariantCulture);
                if (builder.Environment.IsDevelopment())
                {
                    _ = consoleLogger.Filter.ByExcluding(
                        Matching.FromSource("Duende.IdentityServer.Diagnostics.Summary"));
                }
            });
            if (builder.Environment.IsDevelopment())
            {
                _ = lc.WriteTo.Logger(fileLogger =>
                {
                    _ = fileLogger
                        .WriteTo.File("./diagnostics/diagnostic.log", rollingInterval: RollingInterval.Day,
                            fileSizeLimitBytes: 1024 * 1024 * 10, // 10 MB
                            rollOnFileSizeLimit: true,
                            outputTemplate:
                            "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}",
                            formatProvider: CultureInfo.InvariantCulture)
                        .Filter
                        .ByIncludingOnly(Matching.FromSource("Duende.IdentityServer.Diagnostics.Summary"));
                }).Enrich.FromLogContext().ReadFrom.Configuration(builder.Configuration);
            }
        });
        return builder;
    }

    public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
    {
        _ = builder.Services.AddRazorPages();

        builder.AddServiceDefaults();
        
        var migrationsAssembly = typeof(Program).Assembly.GetName().Name;

        _ = builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(builder.Configuration.GetConnectionString("identity-db")));

        _ = builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

        _ = builder.Services
            .AddIdentityServer(options =>
            {
                options.Events.RaiseErrorEvents = true;
                options.Events.RaiseInformationEvents = true;
                options.Events.RaiseFailureEvents = true;
                options.Events.RaiseSuccessEvents = true;

                // Use a large chunk size for diagnostic data in development where it will be redirected to a local file.
                if (builder.Environment.IsDevelopment())
                {
                    options.Diagnostics.ChunkSize = 1024 * 1024 * 10; // 10 MB
                }
            })
            .AddConfigurationStore(options =>
            {
                options.ConfigureDbContext = b => b.UseNpgsql(builder.Configuration.GetConnectionString("identity-db"),
                    sql => sql.MigrationsAssembly(migrationsAssembly));
            })
            .AddOperationalStore(options =>
            {
                // پاکسازی خودکار توکن‌های منقضی شده از دیتابیس (بسیار مهم برای پرفورمنس)
                options.EnableTokenCleanup = true;
                options.TokenCleanupInterval = 3600; // هر یک ساعت
                options.ConfigureDbContext = b => b.UseNpgsql(builder.Configuration.GetConnectionString("identity-db"),
                    sql => sql.MigrationsAssembly(migrationsAssembly));
            })
            .AddAspNetIdentity<ApplicationUser>()
            .AddLicenseSummary();

        _ = builder.Services.AddAuthentication()
            .AddOpenIdConnect("oidc", "Sign-in with demo.duendesoftware.com", options =>
            {
                options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;
                options.SignOutScheme = IdentityServerConstants.SignoutScheme;
                options.SaveTokens = true;

                options.Authority = "https://demo.duendesoftware.com";
                options.ClientId = "interactive.confidential";
                options.ClientSecret = "secret";
                options.ResponseType = "code";

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    NameClaimType = "name",
                    RoleClaimType = "role"
                };
            });

        // add `.PersistKeysTo…()` and `.ProtectKeysWith…()` calls
        // see more at https://docs.duendesoftware.com/general/data-protection
        _ = builder.Services.AddDataProtection()
            .SetApplicationName("IdentityServer");
        
        builder.EnrichNpgsqlDbContext<ApplicationDbContext>();
        builder.EnrichNpgsqlDbContext<PersistedGrantDbContext>();
        builder.EnrichNpgsqlDbContext<ConfigurationDbContext>();

        return builder.Build();
    }

    public static WebApplication ConfigurePipeline(this WebApplication app)
    {
        app.UseForwardedHeaders();  // برای اینکه با gateway کار کنه بعدا بررسی بشه
        
        if (app.Environment.IsDevelopment())
        {
            _ = app.UseDeveloperExceptionPage();
        }

        InitializeDatabase(app);
        _ = app.UseStaticFiles();
        _ = app.UseRouting();
        _ = app.UseIdentityServer();
        _ = app.UseAuthorization();

        _ = app.MapRazorPages()
            .RequireAuthorization();

        app.MapDefaultEndpoints();

        return app;
    }
    private static void InitializeDatabase(IApplicationBuilder app)
    {
        using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>()!.CreateScope())
        {
            serviceScope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>().Database.Migrate();

            var context = serviceScope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();
            context.Database.Migrate();
            if (!context.Clients.Any())
            {
                foreach (var client in Config.Clients)
                {
                    context.Clients.Add(client.ToEntity());
                }
                context.SaveChanges();
            }

            if (!context.IdentityResources.Any())
            {
                foreach (var resource in Config.IdentityResources)
                {
                    context.IdentityResources.Add(resource.ToEntity());
                }
                context.SaveChanges();
            }

            if (!context.ApiScopes.Any())
            {
                foreach (var resource in Config.ApiScopes)
                {
                    context.ApiScopes.Add(resource.ToEntity());
                }
                context.SaveChanges();
            }
        }
    }
    
}