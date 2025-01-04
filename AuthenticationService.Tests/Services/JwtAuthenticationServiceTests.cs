using System.Security.Claims;
using AuthenticationService.Models;
using AuthenticationService.Models.DTOs;
using AuthenticationService.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Moq;
using Xunit;

namespace AuthenticationService.Tests.Services;

public class JwtAuthenticationServiceTests
{
    private readonly Mock<UserManager<ApplicationUser>> _userManagerMock;
    private readonly JwtConfiguration _jwtConfig;
    private readonly JwtAuthenticationService _service;

    public JwtAuthenticationServiceTests()
    {
        // UserManager mock'u hazırla
        var userStoreMock = new Mock<IUserStore<ApplicationUser>>();
        _userManagerMock = new Mock<UserManager<ApplicationUser>>(
            userStoreMock.Object, null, null, null, null, null, null, null, null);

        // JWT yapılandırması
        _jwtConfig = new JwtConfiguration
        {
            Key = "test-key-that-is-long-enough-for-testing-purposes-123!",
            Issuer = "test-issuer",
            Audience = "test-audience",
            ExpirationInMinutes = 60
        };

        _service = new JwtAuthenticationService(_userManagerMock.Object, _jwtConfig);
    }

    [Fact]
    public async Task LoginAsync_WithValidCredentials_ShouldReturnToken()
    {
        // Arrange
        var loginRequest = new LoginRequest
        {
            Username = "test@example.com",
            Password = "Test123!",
            Provider = "jwt"
        };

        var user = new ApplicationUser
        {
            Id = Guid.NewGuid().ToString(),
            UserName = loginRequest.Username,
            Email = loginRequest.Username
        };

        _userManagerMock.Setup(x => x.FindByNameAsync(loginRequest.Username))
            .ReturnsAsync(user);

        _userManagerMock.Setup(x => x.CheckPasswordAsync(user, loginRequest.Password))
            .ReturnsAsync(true);

        // Act
        var result = await _service.LoginAsync(loginRequest);

        // Assert
        result.Should().NotBeNull();
        result.Token.Should().NotBeNullOrEmpty();
        result.ExpiresIn.Should().Be(_jwtConfig.ExpirationInMinutes * 60);
    }

    [Fact]
    public async Task LoginAsync_WithInvalidCredentials_ShouldThrowUnauthorizedAccessException()
    {
        // Arrange
        var loginRequest = new LoginRequest
        {
            Username = "test@example.com",
            Password = "WrongPassword123!",
            Provider = "jwt"
        };

        var user = new ApplicationUser
        {
            UserName = loginRequest.Username,
            Email = loginRequest.Username
        };

        _userManagerMock.Setup(x => x.FindByNameAsync(loginRequest.Username))
            .ReturnsAsync(user);

        _userManagerMock.Setup(x => x.CheckPasswordAsync(user, loginRequest.Password))
            .ReturnsAsync(false);

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(
            () => _service.LoginAsync(loginRequest));
    }

    [Fact]
    public async Task ValidateTokenAsync_WithValidToken_ShouldReturnTrue()
    {
        // Arrange
        var loginRequest = new LoginRequest
        {
            Username = "test@example.com",
            Password = "Test123!",
            Provider = "jwt"
        };

        var user = new ApplicationUser
        {
            Id = Guid.NewGuid().ToString(),
            UserName = loginRequest.Username,
            Email = loginRequest.Username
        };

        _userManagerMock.Setup(x => x.FindByNameAsync(loginRequest.Username))
            .ReturnsAsync(user);

        _userManagerMock.Setup(x => x.CheckPasswordAsync(user, loginRequest.Password))
            .ReturnsAsync(true);

        var loginResult = await _service.LoginAsync(loginRequest);

        // Act
        var isValid = await _service.ValidateTokenAsync(loginResult.Token);

        // Assert
        isValid.Should().BeTrue();
    }

    [Fact]
    public async Task ValidateTokenAsync_WithInvalidToken_ShouldReturnFalse()
    {
        // Arrange
        var invalidToken = "invalid-token";

        // Act
        var isValid = await _service.ValidateTokenAsync(invalidToken);

        // Assert
        isValid.Should().BeFalse();
    }
} 