using Otex.BuildingBlocks.Application.Cqrs;

namespace Otex.Micros.Identity.Application.Identity.Features.Commands.Signup;

public record SignupCommand(
    string TempToken,
    string Password,
    string ConfirmPassword,
    string FirstName,
    string LastName) : ICommand;