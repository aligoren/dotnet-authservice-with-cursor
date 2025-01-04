using System.Security.Claims;
using AuthenticationService.Models;
using AuthenticationService.Models.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace AuthenticationService.Services;

public class OAuth2AuthenticationService : IAuthenticationService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly JwtAuthenticationService _jwtService;
    private readonly OAuth2Configuration _oauth2Config;
    private readonly IConfigurationManager<OpenIdConnectConfiguration> _configManager;

    public OAuth2AuthenticationService(
        UserManager<ApplicationUser> userManager,
        JwtAuthenticationService jwtService,
        OAuth2Configuration oauth2Config)
    {
        _userManager = userManager;
        _jwtService = jwtService;
        _oauth2Config = oauth2Config;

        var documentRetriever = new HttpDocumentRetriever();
        _configManager = new ConfigurationManager<OpenIdConnectConfiguration>(
            $"{oauth2Config.Authority}/.well-known/openid-configuration",
            new OpenIdConnectConfigurationRetriever(),
            documentRetriever
        );
    }

    public async Task<AuthenticationResponse> LoginAsync(LoginRequest request)
    {
        if (string.IsNullOrEmpty(request.Provider))
            throw new ArgumentException("OAuth2 sağlayıcısı belirtilmedi.");

        // Bu metod normalde bir OAuth2 akışının son adımıdır
        // Gerçek uygulamada, bu noktada access token alınmış olmalıdır
        throw new NotImplementedException("OAuth2 login akışı henüz implement edilmedi.");
    }

    public async Task<AuthenticationResponse> RegisterAsync(RegisterRequest request)
    {
        throw new NotImplementedException("OAuth2 servisi üzerinden direkt kayıt yapılamaz.");
    }

    public async Task<bool> ValidateTokenAsync(string token)
    {
        try
        {
            var config = await _configManager.GetConfigurationAsync(CancellationToken.None);
            var tokenHandler = new JwtSecurityTokenHandler();

            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKeys = config.SigningKeys,
                ValidateIssuer = true,
                ValidIssuer = config.Issuer,
                ValidateAudience = true,
                ValidAudience = _oauth2Config.ClientId,
                ClockSkew = TimeSpan.Zero
            };

            tokenHandler.ValidateToken(token, validationParameters, out _);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<string> GetAuthorizationUrlAsync(string state)
    {
        var config = await _configManager.GetConfigurationAsync(CancellationToken.None);
        
        var parameters = new Dictionary<string, string>
        {
            { "client_id", _oauth2Config.ClientId },
            { "redirect_uri", _oauth2Config.RedirectUri },
            { "response_type", "code" },
            { "scope", "openid profile email" },
            { "state", state }
        };

        var queryString = string.Join("&", parameters.Select(p => $"{p.Key}={Uri.EscapeDataString(p.Value)}"));
        return $"{config.AuthorizationEndpoint}?{queryString}";
    }

    public async Task<AuthenticationResponse> HandleCallbackAsync(string code, string state)
    {
        // Token endpoint'inden access token alınması
        // Kullanıcı bilgilerinin alınması
        // Local kullanıcı hesabının oluşturulması/güncellenmesi
        // JWT token'ın oluşturulması
        throw new NotImplementedException();
    }
} 