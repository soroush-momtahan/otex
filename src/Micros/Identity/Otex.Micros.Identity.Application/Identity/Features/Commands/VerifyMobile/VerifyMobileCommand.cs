using Otex.BuildingBlocks.Application.Cqrs;

namespace Otex.Micros.Identity.Application.Identity.Features.Commands.VerifyMobile;

public record VerifyMobileCommand(
    string Code,
    string Mobile) : ICommand<VerifyMobileResult>;