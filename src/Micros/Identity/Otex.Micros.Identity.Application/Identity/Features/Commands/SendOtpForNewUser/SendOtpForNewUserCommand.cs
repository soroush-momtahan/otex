using Otex.BuildingBlocks.Application.Cqrs;

namespace Otex.Micros.Identity.Application.Identity.Features.Commands.SendOtpForNewUser;

public record SendOtpForNewUserCommand(
    string Mobile) : ICommand<SendOtpForNewUserResult>;