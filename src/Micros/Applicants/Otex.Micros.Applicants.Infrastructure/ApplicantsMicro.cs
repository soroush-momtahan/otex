using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Otex.BuildingBlocks.Infrastructure.Outbox;
using Otex.Micros.Applicants.Application.Abstraction;
using Otex.Micros.Applicants.Domain.Cooperation.Repository;
using Otex.Micros.Applicants.Infrastructure.Cooperation.Repository;
using Otex.Micros.Applicants.Infrastructure.Data;

namespace Otex.Micros.Applicants.Infrastructure;

public static class ApplicantsMicro
{
    extension(IHostApplicationBuilder builder)
    {
        public IHostApplicationBuilder AddApplicantMicro(string connString)
        {
            builder.AddDb(connString);
            return builder;
        }

        private void AddDb(string connString)
        {
            builder.Services.AddDbContext<ApplicantsDbContext>((serviceProvider, options) =>
            {
                options.UseNpgsql(builder.Configuration.GetConnectionString(connString));
                options.AddInterceptors(serviceProvider.GetRequiredService<InsertOutboxMessageInterceptor>());
            });
            builder.EnrichNpgsqlDbContext<ApplicantsDbContext>();
            builder.Services.AddScoped<ICooperationRepository, CooperationRepository>();
            builder.Services.AddScoped<IUnitOfWork>(provider => provider.GetRequiredService<ApplicantsDbContext>());
        }
    }
}