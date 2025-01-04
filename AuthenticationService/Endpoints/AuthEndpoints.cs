using AuthenticationService.Models.DTOs;
using AuthenticationService.Services;
using Microsoft.AspNetCore.Http.HttpResults;

namespace AuthenticationService.Endpoints;

public static class AuthEndpoints
{
    public static IEndpointRouteBuilder MapAuthEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/auth");

        group.MapPost("/login", HandleLoginAsync)
            .WithName("Login")
            .WithOpenApi();

        group.MapPost("/register", HandleRegisterAsync)
            .WithName("Register")
            .WithOpenApi();

        group.MapPost("/validate", HandleValidateAsync)
            .WithName("Validate")
            .WithOpenApi();

        return app;
    }

    public static async Task<IResult> HandleLoginAsync(
        LoginRequest request,
        AuthenticationServiceFactory serviceFactory)
    {
        try
        {
            var service = serviceFactory.CreateService(request.Provider, request.ProviderInstance);
            var response = await service.LoginAsync(request);
            return Results.Ok(response);
        }
        catch (UnauthorizedAccessException)
        {
            return Results.Unauthorized();
        }
        catch (Exception ex)
        {
            return Results.BadRequest(new { message = ex.Message });
        }
    }

    public static async Task<IResult> HandleRegisterAsync(
        RegisterRequest request,
        AuthenticationServiceFactory serviceFactory)
    {
        try
        {
            var service = serviceFactory.CreateService();
            var response = await service.RegisterAsync(request);
            return Results.Ok(response);
        }
        catch (Exception ex)
        {
            return Results.BadRequest(new { message = ex.Message });
        }
    }

    public static async Task<IResult> HandleValidateAsync(
        string token,
        AuthenticationServiceFactory serviceFactory)
    {
        try
        {
            var service = serviceFactory.CreateService();
            var isValid = await service.ValidateTokenAsync(token);
            return Results.Ok(isValid);
        }
        catch (Exception ex)
        {
            return Results.BadRequest(new { message = ex.Message });
        }
    }
} 