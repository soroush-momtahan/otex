using Otex.Micros.Applicants.Domain.Cooperation.ValueObjects;

namespace Otex.Micros.Applicants.Domain.Cooperation.Repository;

public interface ICooperationRepository
{
    Task CreateCooperationAsync(Models.Cooperation cooperation, CancellationToken cancellationToken);
}