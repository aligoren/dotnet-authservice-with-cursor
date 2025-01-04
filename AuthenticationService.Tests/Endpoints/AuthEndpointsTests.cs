using AuthenticationService.Endpoints;
using AuthenticationService.Models.DTOs;
using AuthenticationService.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Routing;
using Moq;
using Xunit;

namespace AuthenticationService.Tests.Endpoints;

public class AuthEndpointsTests
{
    private readonly Mock<AuthenticationServiceFactory> _serviceFactoryMock;
    private readonly Mock<IEndpointRouteBuilder> _appMock;

    public AuthEndpointsTests()
    {
        _serviceFactoryMock = new Mock<AuthenticationServiceFactory>(null, null);
        _appMock = new Mock<IEndpointRouteBuilder>();
    }

    [Fact]
    public async Task Login_WithValidCredentials_ShouldReturnOk()
    {
        // Arrange
        var loginRequest = new LoginRequest
        {
            Username = "test@example.com",
            Password = "Test123!",
            Provider = "jwt"
        };

        var authResponse = new AuthenticationResponse
        {
            Token = "test-token",
            ExpiresIn = 3600
        };

        var serviceMock = new Mock<IAuthenticationService>();
        serviceMock.Setup(x => x.LoginAsync(loginRequest))
            .ReturnsAsync(authResponse);

        _serviceFactoryMock.Setup(x => x.CreateService(loginRequest.Provider, loginRequest.ProviderInstance))
            .Returns(serviceMock.Object);

        // Act
        var result = await AuthEndpoints.HandleLoginAsync(loginRequest, _serviceFactoryMock.Object);

        // Assert
        var okResult = Assert.IsType<Ok<AuthenticationResponse>>(result);
        okResult.Value.Should().BeEquivalentTo(authResponse);
    }

    [Fact]
    public async Task Login_WithInvalidCredentials_ShouldReturnUnauthorized()
    {
        // Arrange
        var loginRequest = new LoginRequest
        {
            Username = "test@example.com",
            Password = "WrongPassword123!",
            Provider = "jwt"
        };

        var serviceMock = new Mock<IAuthenticationService>();
        serviceMock.Setup(x => x.LoginAsync(loginRequest))
            .ThrowsAsync(new UnauthorizedAccessException());

        _serviceFactoryMock.Setup(x => x.CreateService(loginRequest.Provider, loginRequest.ProviderInstance))
            .Returns(serviceMock.Object);

        // Act
        var result = await AuthEndpoints.HandleLoginAsync(loginRequest, _serviceFactoryMock.Object);

        // Assert
        Assert.IsType<UnauthorizedHttpResult>(result);
    }

    [Fact]
    public async Task Register_WithValidData_ShouldReturnOk()
    {
        // Arrange
        var registerRequest = new RegisterRequest
        {
            Username = "test@example.com",
            Email = "test@example.com",
            Password = "Test123!",
            FirstName = "Test",
            LastName = "User"
        };

        var authResponse = new AuthenticationResponse
        {
            Token = "test-token",
            ExpiresIn = 3600
        };

        var serviceMock = new Mock<IAuthenticationService>();
        serviceMock.Setup(x => x.RegisterAsync(registerRequest))
            .ReturnsAsync(authResponse);

        _serviceFactoryMock.Setup(x => x.CreateService())
            .Returns(serviceMock.Object);

        // Act
        var result = await AuthEndpoints.HandleRegisterAsync(registerRequest, _serviceFactoryMock.Object);

        // Assert
        var okResult = Assert.IsType<Ok<AuthenticationResponse>>(result);
        okResult.Value.Should().BeEquivalentTo(authResponse);
    }

    [Fact]
    public async Task Validate_WithValidToken_ShouldReturnOk()
    {
        // Arrange
        var token = "valid-token";
        var serviceMock = new Mock<IAuthenticationService>();
        serviceMock.Setup(x => x.ValidateTokenAsync(token))
            .ReturnsAsync(true);

        _serviceFactoryMock.Setup(x => x.CreateService())
            .Returns(serviceMock.Object);

        // Act
        var result = await AuthEndpoints.HandleValidateAsync(token, _serviceFactoryMock.Object);

        // Assert
        var okResult = Assert.IsType<Ok<bool>>(result);
        okResult.Value.Should().BeTrue();
    }
} 