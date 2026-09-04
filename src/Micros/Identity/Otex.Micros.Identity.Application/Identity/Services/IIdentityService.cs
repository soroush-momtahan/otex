using Otex.BuildingBlocks.Domain.Results;

namespace Otex.Micros.Identity.Application.Identity.Services;

public interface IIdentityService
{
    Task<string?> FindByMobileAsync(string mobile);
    Task<Result<string>> SignupWithMobileAsync(string mobile, string password);
    Task<Result> SigninByMobileAsync(string mobile, string password);
}