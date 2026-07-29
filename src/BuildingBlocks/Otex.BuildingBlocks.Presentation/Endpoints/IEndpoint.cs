using Microsoft.AspNetCore.Routing;

namespace Otex.Services.Shared.Presentation.Endpoints;

public interface IEndpoint
{
    void MapEndpoint(IEndpointRouteBuilder app);
}
