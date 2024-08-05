using Microsoft.AspNetCore.Routing;

namespace Crosscutting.Api.Endpoints;

public interface IEndpoint
{
    void MapEndpoint(IEndpointRouteBuilder app);
}
