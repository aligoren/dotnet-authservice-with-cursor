using AuthenticationService.Models;
using AuthenticationService.Models.DTOs;

namespace AuthenticationService.Services;

public interface IAuthenticationService
{
    Task<AuthenticationResponse> LoginAsync(LoginRequest request);
    Task<AuthenticationResponse> RegisterAsync(RegisterRequest request);
    Task<bool> ValidateTokenAsync(string token);
} 