using MassTransit.Initializers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Otex.BuildingBlocks.Domain.Errors;
using Otex.BuildingBlocks.Domain.Results;
using Otex.Micros.Identity.Application.Identity.Services;

namespace Otex.Micros.Identity.Infrastructure.Identity.Services;

public class IdentityService(
    UserManager<IdentityUser> userManager,
    SignInManager<IdentityUser> signInManager) : IIdentityService
{
    public async Task<string?> FindByMobileAsync(string mobile)
    {
        return await userManager.Users
            .FirstOrDefaultAsync(x => x.PhoneNumber == mobile)
            .Select(i => i?.Id);
    }

    public async Task<Result<string>> SignupWithMobileAsync(string mobile, string password)
    {
        var identityUser = new IdentityUser
        {
            UserName = mobile,
            PhoneNumber = mobile,
            PhoneNumberConfirmed = true
        };
        List<Error> errors = [];
        var identityUserOrError = await userManager.CreateAsync(identityUser, password);
        if (!identityUserOrError.Succeeded)
        {
            foreach (var err in identityUserOrError.Errors)
            {
                errors.Add(new Error(err.Code, err.Description, ErrorType.Validation));
            }

            return Result.Failure<string>(Error.Combine(errors.ToArray()));
        }
        
        await signInManager.PasswordSignInAsync(identityUser, password, true, false);

        // return userManager.Users.FirstOrDefault(x => x.PhoneNumber == mobile)?.Id;
        return identityUser.Id;
    }

    public Task<Result> SigninByMobileAsync(string mobile, string password)
    {
        throw new NotImplementedException();
    }
}