namespace Otex.Micros.Identity.Application.Identity.Features.Commands.VerifyMobile;

public record VerifyMobileResult(
    bool IsNewUser,
    string TempToken);