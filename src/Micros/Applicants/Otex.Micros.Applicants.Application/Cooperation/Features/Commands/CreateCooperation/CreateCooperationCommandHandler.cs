using Otex.BuildingBlocks.Application.Cqrs;
using Otex.BuildingBlocks.Domain.Results;
using Otex.Micros.Applicants.Application.Abstraction;
using Otex.Micros.Applicants.Domain.Cooperation.Repository;
using Otex.Micros.Applicants.Domain.Cooperation.ValueObjects;

namespace Otex.Micros.Applicants.Application.Cooperation.Features.Commands.CreateCooperation;

internal sealed class CreateCooperationCommandHandler(
    ICooperationRepository cooperationRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<CreateCooperationCommand>
{
    public async Task<Result> Handle(CreateCooperationCommand command, CancellationToken cancellationToken)
    {
        List<Result> results = [];
        Result<Fullname> fullnameOrError = Fullname.From(command.FirstName, command.LastName);
        if (fullnameOrError.IsFailure)
        {
            results.Add(Result.Failure(fullnameOrError.Error));
        }

        Result<Mobile> mobileOrError = Mobile.From(command.Mobile);
        if (mobileOrError.IsFailure)
        {
            results.Add(Result.Failure(mobileOrError.Error));
        }
        
        Location location = new Location(command.Province, command.City);

        ReservationDateTime? reserveDateTime = null;
        if (command.ReservationDateTime.HasValue)
        {
            Result<ReservationDateTime> reservationDateTimeOrError = ReservationDateTime.From(command.ReservationDateTime.Value);
            if (reservationDateTimeOrError.IsFailure)
            {
                results.Add(Result.Failure(reservationDateTimeOrError.Error));
            }
            reserveDateTime = reservationDateTimeOrError.Value;
        }

        Description? description = null;
        if (command.Description != null)
        {
            description = new Description(command.Description);
        }

        if (results.Count != 0)
        {
            return Result.Combine(results);
        }

        var cooperation = Domain.Cooperation.Models.Cooperation.Create(
            fullnameOrError.Value,
            mobileOrError.Value,
            location,
            command.TypeOfActivity,
            reserveDateTime, description);
        
        await cooperationRepository.CreateCooperationAsync(cooperation, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        
        return Result.Success();
    }
}