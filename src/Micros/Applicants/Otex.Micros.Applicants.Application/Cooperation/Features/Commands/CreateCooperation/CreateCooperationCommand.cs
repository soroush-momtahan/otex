using Otex.BuildingBlocks.Application.Cqrs;
using Otex.Micros.Applicants.Domain.Cooperation.Enums;

namespace Otex.Micros.Applicants.Application.Cooperation.Features.Commands.CreateCooperation;

public record CreateCooperationCommand(
    string FirstName,
    string LastName,
    string Mobile,
    string Province,
    string City,
    TypeOfActivity TypeOfActivity,
    string? Description,
    DateTime? ReservationDateTime) : ICommand;