using AuthenticationService.Models.DTOs;
using Microsoft.Extensions.DependencyInjection;

namespace AuthenticationService.Services;

public class AuthenticationServiceFactory
{
    private readonly IServiceProvider _serviceProvider;
    private readonly MultiTenantConfiguration _configuration;

    public AuthenticationServiceFactory(
        IServiceProvider serviceProvider,
        MultiTenantConfiguration configuration)
    {
        _serviceProvider = serviceProvider;
        _configuration = configuration;
    }

    public IAuthenticationService CreateService(string? provider = null, string? providerInstance = null)
    {
        IAuthenticationService baseService = (provider?.ToLower()) switch
        {
            "ldap" => CreateLdapService(providerInstance),
            "oauth2" => _serviceProvider.GetRequiredService<OAuth2AuthenticationService>(),
            "entraid" => CreateEntraIdService(providerInstance),
            _ => _serviceProvider.GetRequiredService<JwtAuthenticationService>()
        };

        return baseService;
    }

    private LdapAuthenticationService CreateLdapService(string? instance)
    {
        var userManager = _serviceProvider.GetRequiredService<Microsoft.AspNetCore.Identity.UserManager<Models.ApplicationUser>>();
        var jwtService = _serviceProvider.GetRequiredService<JwtAuthenticationService>();
        var service = new LdapAuthenticationService(userManager, jwtService);

        // Varsayılan olarak ilk yapılandırmayı kullan veya belirtilen instance'ı bul
        var ldapConfig = string.IsNullOrEmpty(instance)
            ? _configuration.LdapInstances.First().Value
            : _configuration.LdapInstances.GetValueOrDefault(instance)
                ?? throw new ArgumentException($"LDAP instance '{instance}' bulunamadı.");

        service.SetConfiguration(ldapConfig);
        return service;
    }

    private EntraIdAuthenticationService CreateEntraIdService(string? instance)
    {
        var userManager = _serviceProvider.GetRequiredService<Microsoft.AspNetCore.Identity.UserManager<Models.ApplicationUser>>();
        var jwtService = _serviceProvider.GetRequiredService<JwtAuthenticationService>();
        var service = new EntraIdAuthenticationService(userManager, jwtService);

        // Varsayılan olarak ilk yapılandırmayı kullan veya belirtilen instance'ı bul
        var entraConfig = string.IsNullOrEmpty(instance)
            ? _configuration.EntraIdInstances.First().Value
            : _configuration.EntraIdInstances.GetValueOrDefault(instance)
                ?? throw new ArgumentException($"Entra ID instance '{instance}' bulunamadı.");

        service.SetConfiguration(entraConfig);
        return service;
    }
} 