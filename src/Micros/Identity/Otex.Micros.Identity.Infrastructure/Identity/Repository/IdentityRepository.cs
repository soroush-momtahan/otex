using Microsoft.EntityFrameworkCore;
using Otex.Micros.Identity.Domain.Identity.Models;
using Otex.Micros.Identity.Domain.Identity.Repository;
using Otex.Micros.Identity.Infrastructure.Data;

namespace Otex.Micros.Identity.Infrastructure.Identity.Repository;

public class IdentityRepository(
    IdentityUserDbContext userDbContext) : IIdentityRepository
{
    public async Task CreateAsync(User user, CancellationToken cancellationToken)
    {
        await userDbContext.Users.AddAsync(user, cancellationToken);
    }

    public async Task<User?> FindByMobileAsync(string mobile, CancellationToken cancellationToken)
    {
        return await userDbContext.Users.FirstOrDefaultAsync(x => x.Mobile.Value == mobile, cancellationToken);
    }
}