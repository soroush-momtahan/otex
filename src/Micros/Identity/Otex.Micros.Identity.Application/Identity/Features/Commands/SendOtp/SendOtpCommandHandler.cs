using Otex.BuildingBlocks.Application.Caching;
using Otex.BuildingBlocks.Application.Cqrs;
using Otex.BuildingBlocks.Domain.Results;
using Otex.Micros.Identity.Application.Identity.Services;
using Otex.Micros.Identity.Domain.Identity.Errors;

namespace Otex.Micros.Identity.Application.Identity.Features.Commands.SendOtp;

public sealed class SendOtpCommandHandler(OtpService otpService) 
    : ICommandHandler<SendOtpCommand>
{
    public async Task<Result> Handle(SendOtpCommand command, CancellationToken cancellationToken)
    {
        return await otpService.SendOtpAsync(command.Mobile, cancellationToken);
    }
}