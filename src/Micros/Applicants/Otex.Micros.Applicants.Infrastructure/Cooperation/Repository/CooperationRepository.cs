using Otex.Micros.Applicants.Domain.Cooperation.Repository;
using Otex.Micros.Applicants.Infrastructure.Data;

namespace Otex.Micros.Applicants.Infrastructure.Cooperation.Repository;

public class CooperationRepository(
    ApplicantsDbContext dbContext) : ICooperationRepository
{
    public async Task CreateCooperationAsync(Domain.Cooperation.Models.Cooperation cooperation, CancellationToken cancellationToken)
    {
        await dbContext.AddAsync(cooperation, cancellationToken);
    }
}