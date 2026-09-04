using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Routing;
using Otex.BuildingBlocks.Domain.Results;
using Otex.BuildingBlocks.Presentation.ApiResults;
using Otex.BuildingBlocks.Presentation.Endpoints;
using Otex.Micros.Applicants.Application.Cooperation.Features.Commands.CreateCooperation;
using Otex.Micros.Applicants.Domain.Cooperation.Enums;

namespace Otex.Micros.Applicants.Presentation.Cooperation.Endpoints.CreateCooperation;

internal sealed class CreateCooperationEndpoint : IEndpoint
{
    public record CreateCooperationRequest(
        string Firstname,
        string Lastname,
        string Mobile,
        string Province,
        string City,
        TypeOfActivity TypeOfActivity,
        string? Description,
        DateTime? ReservationDateTime);

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/cooperation", async (
                CreateCooperationRequest req,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                Result result = await sender.Send(ToCommand(req), cancellationToken);
                return result.Match(Results.Created, ApiResults.Problem);
            })
            .WithTags(Tags.Cooperation);
    }

    private static CreateCooperationCommand ToCommand(CreateCooperationRequest req)
    {
        return new CreateCooperationCommand(
            req.Firstname,
            req.Lastname,
            req.Mobile,
            req.Province,
            req.City,
            req.TypeOfActivity,
            req.Description,
            req.ReservationDateTime);
    }
}