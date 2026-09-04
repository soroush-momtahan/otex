using Microsoft.AspNetCore.Routing;

namespace Otex.BuildingBlocks.Presentation.Endpoints;

public interface IEndpoint
{
    void MapEndpoint(IEndpointRouteBuilder app);
}
