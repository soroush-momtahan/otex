using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Otex.BuildingBlocks.Infrastructure.Extensions;
using Otex.BuildingBlocks.Infrastructure.Outbox;
using Otex.Micros.Identity.Application.Abstraction;
using Otex.Micros.Identity.Application.Identity.Services;
using Otex.Micros.Identity.Domain.Identity.Models;
using Otex.Micros.Identity.Domain.Identity.Repository;
using Otex.Micros.Identity.Infrastructure.Data;
using Otex.Micros.Identity.Infrastructure.Identity.ErrorDescriptor;
using Otex.Micros.Identity.Infrastructure.Identity.Repository;
using Otex.Micros.Identity.Infrastructure.Identity.Services;

namespace Otex.Micros.Identity.Infrastructure;

public static class IdentityMicro
{
    extension(IHostApplicationBuilder builder)
    {
        public IHostApplicationBuilder AddIdentityMicro(
            string dbConnString,
            string cacheConnString)
        {
            builder.AddRedisCacheServices(cacheConnString);
            builder
                .AddDb(dbConnString)
                .AddAspIdentityServices();
            // builder.Services.AddScoped<InsertOutboxMessageInterceptor>();
            builder.Services.AddScoped<OtpService>();
            builder.Services.AddSingleton<IOtpGeneratorService, OtpGeneratorService>();
            builder.Services.AddScoped<IOtpSenderService, OtpSenderService>();
            
            return builder;
        }

        private IHostApplicationBuilder AddAspIdentityServices()
        {
            builder.Services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequiredUniqueChars = User.MinPasswordUniqueChars; // کاربر باید حداقل ۴ کاراکتر متفاوت استفاده کند
                options.Password.RequiredLength = User.MinPasswordLength;
            });
            
            builder.Services.AddDbContext<AspIdentityDbContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString("identity-db")));
        
            builder.Services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<AspIdentityDbContext>()
                .AddErrorDescriber<CustomIdentityErrorDescriber>()
                .AddDefaultTokenProviders();
            
            builder.EnrichNpgsqlDbContext<AspIdentityDbContext>();
        
            builder.Services.AddScoped<IIdentityService, IdentityService>();
        
            return builder;
        }

        private IHostApplicationBuilder AddDb(string connString)
        {
            builder.Services.AddDbContext<IdentityUserDbContext>((serviceProvider, options) =>
            {
                options.UseNpgsql(builder.Configuration.GetConnectionString(connString));
                options.AddInterceptors(serviceProvider.GetRequiredService<InsertOutboxMessageInterceptor>());
            });
            builder.EnrichNpgsqlDbContext<IdentityUserDbContext>();
            builder.Services.AddScoped<IIdentityRepository, IdentityRepository>();
            builder.Services.AddScoped<IUnitOfWork>(provider => provider.GetRequiredService<IdentityUserDbContext>());
            return builder;
        }
    }
}