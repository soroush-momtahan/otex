using Otex.BuildingBlocks.Application.Cqrs;
using Otex.BuildingBlocks.Domain.Results;
using Otex.Micros.Identity.Application.Identity.Services;
using Otex.Micros.Identity.Domain.Identity.Models;
using Otex.Micros.Identity.Domain.Identity.Repository;

namespace Otex.Micros.Identity.Application.Identity.Features.Commands.SendOtpForNewUser;

internal sealed class SendOtpForNewUserCommandHandler(
    IIdentityRepository identityRepository,
    OtpService otpService) : ICommandHandler<SendOtpForNewUserCommand, SendOtpForNewUserResult>
{
    public async Task<Result<SendOtpForNewUserResult>> Handle(SendOtpForNewUserCommand command, CancellationToken cancellationToken)
    {
        User? user = await identityRepository.FindByMobileAsync(command.Mobile, cancellationToken);
        if (user is not null)
        {
            return new SendOtpForNewUserResult(true);
        }

        Result sendOtpOrError = await otpService.SendOtpAsync(command.Mobile, cancellationToken);
        if (sendOtpOrError.IsFailure)
        {
            return Result.Failure<SendOtpForNewUserResult>(sendOtpOrError.Error);
        }
        
        return new SendOtpForNewUserResult(false);
    }
}