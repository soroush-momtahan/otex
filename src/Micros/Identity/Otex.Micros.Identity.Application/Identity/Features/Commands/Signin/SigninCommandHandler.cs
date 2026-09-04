using Otex.BuildingBlocks.Application.Cqrs;
using Otex.BuildingBlocks.Domain.Results;
using Otex.Micros.Identity.Application.Identity.Services;

namespace Otex.Micros.Identity.Application.Identity.Features.Commands.Signin;

public sealed class SigninCommandHandler(
    IIdentityService identityService) : ICommandHandler<SigninCommand>
{
    public async Task<Result> Handle(SigninCommand command, CancellationToken cancellationToken)
    {
        Result signinOrError = await identityService.SigninByMobileAsync(command.Mobile, command.Password);
        if (signinOrError.IsFailure)
        {
            return Result.Failure(signinOrError.Error);
        }

        return Result.Success();
    }
}