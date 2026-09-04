using Otex.Micros.Identity.Domain.Identity.Models;

namespace Otex.Micros.Identity.Domain.Identity.Repository;

public interface IIdentityRepository
{
    Task CreateAsync(User user, CancellationToken cancellationToken);
    Task<User?> FindByMobileAsync(string mobile, CancellationToken cancellationToken);
}