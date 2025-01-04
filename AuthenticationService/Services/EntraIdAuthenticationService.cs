using System.Security.Claims;
using AuthenticationService.Models;
using AuthenticationService.Models.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication;

namespace AuthenticationService.Services;

public class EntraIdAuthenticationService : IAuthenticationService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly JwtAuthenticationService _jwtService;
    private EntraIdConfiguration? _configuration;

    public EntraIdAuthenticationService(
        UserManager<ApplicationUser> userManager,
        JwtAuthenticationService jwtService)
    {
        _userManager = userManager;
        _jwtService = jwtService;
    }

    public void SetConfiguration(EntraIdConfiguration configuration)
    {
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
    }

    public Task<AuthenticationResponse> LoginAsync(LoginRequest request)
    {
        throw new NotImplementedException("Entra ID login should be initiated through the web interface");
    }

    public Task<AuthenticationResponse> RegisterAsync(RegisterRequest request)
    {
        throw new NotImplementedException("Registration through Entra ID is not supported");
    }

    public Task<bool> ValidateTokenAsync(string token)
    {
        return _jwtService.ValidateTokenAsync(token);
    }

    public async Task<AuthenticationResponse> HandleCallbackAsync(AuthenticationProperties properties)
    {
        if (_configuration == null)
            throw new InvalidOperationException("Entra ID configuration is not set");

        throw new NotImplementedException("Entra ID callback handling is not implemented yet");
    }
} 