namespace AuthenticationService.Models.DTOs;

public record LoginRequest(string Username, string Password, string? Provider = null, string? ProviderInstance = null);

public record RegisterRequest(
    string Username,
    string Email,
    string Password,
    string FirstName,
    string LastName);

public record AuthenticationResponse(
    string Token,
    string Username,
    string Email,
    List<string> Roles,
    DateTime ExpiresAt);

public record LdapConfiguration(
    string Name,
    string Server,
    int Port,
    string BaseDn,
    string Domain);

public record JwtConfiguration(
    string Issuer,
    string Audience,
    string Key,
    int ExpirationInMinutes);

public record OAuth2Configuration(
    string ClientId,
    string ClientSecret,
    string Authority,
    string RedirectUri);

public record EntraIdConfiguration(
    string Name,
    string TenantId,
    string ClientId,
    string ClientSecret,
    string Instance,
    string CallbackPath);

public record MultiTenantConfiguration
{
    public required JwtConfiguration Jwt { get; init; }
    public required Dictionary<string, LdapConfiguration> LdapInstances { get; init; }
    public required OAuth2Configuration OAuth2 { get; init; }
    public required Dictionary<string, EntraIdConfiguration> EntraIdInstances { get; init; }
} 