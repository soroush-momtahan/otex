using Otex.BuildingBlocks.Application.Cqrs;

namespace Otex.Micros.Identity.Application.Identity.Features.Commands.Signin;

public record SigninCommand(
    string Mobile,
    string Password) : ICommand;