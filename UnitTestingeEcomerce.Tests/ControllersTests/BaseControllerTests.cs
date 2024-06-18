using eEcomerce.BackEnd.Controllers;
using eEcomerce.BackEnd.Entities.User;
using eEcomerce.BackEnd.Services.User;

using Microsoft.AspNetCore.Http;

using Moq;

using System.Security.Claims;

using UnitTestingeEcomerce.Tests.Utils;

namespace UnitTestingeEcomerce.Tests.ControllersTests;

public class BaseControllerTests
{
    private readonly Mock<IHttpContextAccessor> _mockContextAccessor;
    private readonly Mock<IUserService> _mockUserService;
    private readonly TestBaseController _controller;
    private readonly UserTesterBuilder _userBuilder;

    public BaseControllerTests()
    {
        _mockContextAccessor = new Mock<IHttpContextAccessor>();
        _mockUserService = new Mock<IUserService>();
        _controller = new TestBaseController(_mockContextAccessor.Object, _mockUserService.Object);
        _userBuilder = new UserTesterBuilder(_mockContextAccessor);
    }

    [Fact]
    public void GetUserIdFromToken_UserIdClaimExists_ReturnsUserId()
    {
        // Arrange
        string userId = Guid.NewGuid().ToString();
        _userBuilder.WithId(Guid.Parse(userId)).SetupUserInHttpContext();

        // Act
        Guid? result = _controller.GetUserIdFromToken();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(Guid.Parse(userId), result);
    }

    [Fact]
    public void GetUserIdFromToken_UserIdClaimDoesNotExist_ReturnsNull()
    {
        // Arrange
        ClaimsPrincipal claimsPrincipal = new(new ClaimsIdentity());
        DefaultHttpContext context = new() { User = claimsPrincipal };

        _mockContextAccessor.Setup(x => x.HttpContext).Returns(context);

        // Act
        Guid? result = _controller.GetUserIdFromToken();

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void GetUserIdFromToken_InvalidUserIdClaim_ReturnsNull()
    {
        // Arrange
        List<Claim> claims = [new Claim("UserId", "invalid-guid")];
        ClaimsIdentity identity = new(claims, "TestAuthType");
        ClaimsPrincipal claimsPrincipal = new(identity);
        DefaultHttpContext context = new() { User = claimsPrincipal };

        _mockContextAccessor.Setup(x => x.HttpContext).Returns(context);

        // Act
        Guid? result = _controller.GetUserIdFromToken();

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void ValidateUserId_UserIdIsValid_ReturnsTrue()
    {
        // Arrange
        Guid userId = Guid.NewGuid();
        _mockUserService.Setup(x => x.GetUserById(userId)).Returns(new User());

        // Act
        bool result = _controller.ValidateUserId(userId);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void ValidateUserId_UserIdIsNull_ReturnsFalse()
    {
        // Act
        bool result = _controller.ValidateUserId(null);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void ValidateUserId_UserDoesNotExist_ReturnsFalse()
    {
        // Arrange
        Guid userId = Guid.NewGuid();
        _mockUserService.Setup(x => x.GetUserById(userId)).Returns((User) null);

        // Act
        bool result = _controller.ValidateUserId(userId);

        // Assert
        Assert.False(result);
    }
}

public class TestBaseController(IHttpContextAccessor httpContextAccessor, IUserService userService)
    : BaseController(httpContextAccessor, userService)
{
    public new Guid? GetUserIdFromToken()
    {
        return base.GetUserIdFromToken();
    }

    public new bool ValidateUserId(Guid? userId)
    {
        return base.ValidateUserId(userId);
    }
}