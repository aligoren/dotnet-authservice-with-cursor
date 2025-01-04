using AuthenticationService.Models;
using AuthenticationService.Models.DTOs;
using Microsoft.AspNetCore.Identity;

namespace AuthenticationService.Services;

public class LdapAuthenticationService : IAuthenticationService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly JwtAuthenticationService _jwtService;
    private LdapConfiguration? _configuration;

    public LdapAuthenticationService(
        UserManager<ApplicationUser> userManager,
        JwtAuthenticationService jwtService)
    {
        _userManager = userManager;
        _jwtService = jwtService;
    }

    public void SetConfiguration(LdapConfiguration configuration)
    {
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
    }

    public async Task<AuthenticationResponse> LoginAsync(LoginRequest request)
    {
        if (_configuration == null)
            throw new InvalidOperationException("LDAP configuration is not set");

        // LDAP kimlik doğrulama işlemleri burada yapılacak
        throw new NotImplementedException("LDAP authentication is not implemented yet");
    }

    public Task<AuthenticationResponse> RegisterAsync(RegisterRequest request)
    {
        throw new NotImplementedException("Registration through LDAP is not supported");
    }

    public Task<bool> ValidateTokenAsync(string token)
    {
        return _jwtService.ValidateTokenAsync(token);
    }
} 