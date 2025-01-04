using AuthenticationService.Endpoints;
using FluentAssertions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Routing;
using Moq;
using Xunit;

namespace AuthenticationService.Tests.Endpoints;

public class TestEndpointsTests
{
    private readonly Mock<IEndpointRouteBuilder> _appMock;

    public TestEndpointsTests()
    {
        _appMock = new Mock<IEndpointRouteBuilder>();
    }

    [Fact]
    public void Test_ShouldReturnSuccessMessage()
    {
        // Act
        var result = TestEndpoints.HandleTestRequest();

        // Assert
        var okResult = Assert.IsType<Ok<object>>(result);
        var response = okResult.Value as dynamic;
        ((string)response.message).Should().Be("Başarıyla kimlik doğrulandı!");
    }
} 