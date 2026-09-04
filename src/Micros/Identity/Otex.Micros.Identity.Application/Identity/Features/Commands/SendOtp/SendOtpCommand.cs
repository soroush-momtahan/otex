using Otex.BuildingBlocks.Application.Cqrs;

namespace Otex.Micros.Identity.Application.Identity.Features.Commands.SendOtp;

public record SendOtpCommand(string Mobile) : ICommand;