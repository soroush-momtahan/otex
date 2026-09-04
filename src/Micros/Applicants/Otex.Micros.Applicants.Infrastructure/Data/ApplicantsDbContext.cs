using Microsoft.EntityFrameworkCore;
using Otex.Micros.Applicants.Application.Abstraction;
using Otex.Micros.Applicants.Infrastructure.Cooperation.Configurations;

namespace Otex.Micros.Applicants.Infrastructure.Data;

public sealed class ApplicantsDbContext(DbContextOptions<ApplicantsDbContext> options) : DbContext(options), IUnitOfWork
{
    public DbSet<Domain.Cooperation.Models.Cooperation> Cooperation => Set<Domain.Cooperation.Models.Cooperation>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfiguration(new CooperationConfiguration());
        modelBuilder.HasDefaultSchema(Schemas.Cooperation);
    }
}