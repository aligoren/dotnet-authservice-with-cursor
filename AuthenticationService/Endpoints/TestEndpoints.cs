using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;

namespace AuthenticationService.Endpoints;

public static class TestEndpoints
{
    public static IEndpointRouteBuilder MapTestEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/auth");

        group.MapGet("/test", [Authorize] () => HandleTestRequest())
            .WithName("Test")
            .WithOpenApi();

        return app;
    }

    public static IResult HandleTestRequest()
    {
        return Results.Ok(new { message = "Başarıyla kimlik doğrulandı!" });
    }
} 