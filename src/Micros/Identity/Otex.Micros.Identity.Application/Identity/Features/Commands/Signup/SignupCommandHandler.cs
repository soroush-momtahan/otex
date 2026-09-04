using Otex.BuildingBlocks.Application.Caching;
using Otex.BuildingBlocks.Application.Cqrs;
using Otex.BuildingBlocks.Domain.Exceptions;
using Otex.BuildingBlocks.Domain.Results;
using Otex.Micros.Identity.Application.Abstraction;
using Otex.Micros.Identity.Application.Identity.Services;
using Otex.Micros.Identity.Domain.Identity.Errors;
using Otex.Micros.Identity.Domain.Identity.Models;
using Otex.Micros.Identity.Domain.Identity.Repository;
using Otex.Micros.Identity.Domain.Identity.ValueObjects;

namespace Otex.Micros.Identity.Application.Identity.Features.Commands.Signup;

public sealed class SignupCommandHandler(
    IIdentityRepository identityRepository,
    ICacheService cacheService,
    IIdentityService identityService,
    IUnitOfWork unitOfWork) : ICommandHandler<SignupCommand>
{
    public async Task<Result> Handle(SignupCommand command, CancellationToken cancellationToken)
    {
        string? mobile = await cacheService.GetAsync<string>($"Verified:{command.TempToken}", cancellationToken);
        if (mobile is null)
        {
            return Result.Failure(IdentityErrors.TempVerifyTokenInvalid);
        }

        User? user = await identityRepository.FindByMobileAsync(mobile, cancellationToken);
        if (user is not null)
        {
            return Result.Failure(IdentityErrors.DuplicateMobile(mobile));
        }
        
        Result<FullName> fullNameOrError = FullName.From(command.FirstName, command.LastName);
        if (fullNameOrError.IsFailure)
        {
            return Result.Failure(fullNameOrError.Error);
        }
        
        Result<Mobile> mobileOrError = Mobile.From(mobile);
        if (mobileOrError.IsFailure)
        {
            return Result.Failure(mobileOrError.Error);
        }
        
        await unitOfWork.ExecuteTransactionAsync(async () =>
        {
            
            var identityIdOrErrors = await identityService.SignupWithMobileAsync(mobile, command.Password);
            if (identityIdOrErrors.IsFailure)
            {
                throw new OtexException(nameof(identityService.SignupWithMobileAsync), identityIdOrErrors.Error);
            }

            var identityUserId = new IdentityUserId(Guid.Parse(identityIdOrErrors.Value));
            
            User user = User.Create(identityUserId, fullNameOrError.Value, mobileOrError.Value);
            
            await identityRepository.CreateAsync(user, cancellationToken);
            
            await unitOfWork.SaveChangesAsync(cancellationToken);
        
        }, cancellationToken);
        
        return Result.Success();
    }
}