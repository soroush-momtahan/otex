using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Otex.Micros.Identity.Infrastructure.Data;

public class AspIdentityDbContext(DbContextOptions<AspIdentityDbContext> options) : IdentityDbContext<IdentityUser>(options);