using AuthenticationService.Models.DTOs;
using AuthenticationService.Services;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xunit;

namespace AuthenticationService.Tests.Services;

public class AuthenticationServiceFactoryTests
{
    private readonly Mock<IServiceProvider> _serviceProviderMock;
    private readonly MultiTenantConfiguration _configuration;
    private readonly AuthenticationServiceFactory _factory;

    public AuthenticationServiceFactoryTests()
    {
        _serviceProviderMock = new Mock<IServiceProvider>();
        _configuration = new MultiTenantConfiguration
        {
            LdapInstances = new Dictionary<string, LdapConfiguration>
            {
                ["mainoffice"] = new LdapConfiguration
                {
                    Server = "ldap.example.com",
                    Port = 389,
                    BaseDn = "dc=example,dc=com"
                }
            },
            EntraIdInstances = new Dictionary<string, EntraIdConfiguration>
            {
                ["main"] = new EntraIdConfiguration
                {
                    TenantId = "tenant-id",
                    ClientId = "client-id",
                    ClientSecret = "client-secret",
                    Instance = "https://login.microsoftonline.com/",
                    CallbackPath = "/signin-oidc"
                }
            }
        };

        _factory = new AuthenticationServiceFactory(_serviceProviderMock.Object, _configuration);
    }

    [Fact]
    public void CreateService_WithNoProvider_ShouldReturnJwtService()
    {
        // Arrange
        var jwtServiceMock = new Mock<JwtAuthenticationService>(null, null);
        _serviceProviderMock.Setup(x => x.GetService(typeof(JwtAuthenticationService)))
            .Returns(jwtServiceMock.Object);

        // Act
        var service = _factory.CreateService();

        // Assert
        service.Should().BeOfType<JwtAuthenticationService>();
    }

    [Fact]
    public void CreateService_WithLdapProvider_ShouldReturnLdapService()
    {
        // Arrange
        var userManagerMock = new Mock<Microsoft.AspNetCore.Identity.UserManager<Models.ApplicationUser>>(
            new Mock<Microsoft.AspNetCore.Identity.IUserStore<Models.ApplicationUser>>().Object,
            null, null, null, null, null, null, null, null);
        var jwtServiceMock = new Mock<JwtAuthenticationService>(null, null);

        _serviceProviderMock.Setup(x => x.GetService(typeof(Microsoft.AspNetCore.Identity.UserManager<Models.ApplicationUser>)))
            .Returns(userManagerMock.Object);
        _serviceProviderMock.Setup(x => x.GetService(typeof(JwtAuthenticationService)))
            .Returns(jwtServiceMock.Object);

        // Act
        var service = _factory.CreateService("ldap", "mainoffice");

        // Assert
        service.Should().BeOfType<LdapAuthenticationService>();
    }

    [Fact]
    public void CreateService_WithInvalidLdapInstance_ShouldThrowArgumentException()
    {
        // Act & Assert
        var action = () => _factory.CreateService("ldap", "invalid-instance");
        action.Should().Throw<ArgumentException>()
            .WithMessage("LDAP instance 'invalid-instance' bulunamadı.");
    }

    [Fact]
    public void CreateService_WithEntraIdProvider_ShouldReturnEntraIdService()
    {
        // Arrange
        var userManagerMock = new Mock<Microsoft.AspNetCore.Identity.UserManager<Models.ApplicationUser>>(
            new Mock<Microsoft.AspNetCore.Identity.IUserStore<Models.ApplicationUser>>().Object,
            null, null, null, null, null, null, null, null);
        var jwtServiceMock = new Mock<JwtAuthenticationService>(null, null);

        _serviceProviderMock.Setup(x => x.GetService(typeof(Microsoft.AspNetCore.Identity.UserManager<Models.ApplicationUser>)))
            .Returns(userManagerMock.Object);
        _serviceProviderMock.Setup(x => x.GetService(typeof(JwtAuthenticationService)))
            .Returns(jwtServiceMock.Object);

        // Act
        var service = _factory.CreateService("entraid", "main");

        // Assert
        service.Should().BeOfType<EntraIdAuthenticationService>();
    }

    [Fact]
    public void CreateService_WithInvalidEntraIdInstance_ShouldThrowArgumentException()
    {
        // Act & Assert
        var action = () => _factory.CreateService("entraid", "invalid-instance");
        action.Should().Throw<ArgumentException>()
            .WithMessage("Entra ID instance 'invalid-instance' bulunamadı.");
    }
} 