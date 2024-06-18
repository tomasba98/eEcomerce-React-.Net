using eEcomerce.BackEnd.Controllers;
using eEcomerce.BackEnd.Entities.User;
using eEcomerce.BackEnd.Models.Authentication;
using eEcomerce.BackEnd.Services.Authentication;
using eEcomerce.BackEnd.Services.User;
using eEcomerce.BackEnd.Utils;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Moq;

namespace UnitTestingeEcomerce.Tests.ControllersTests;

public class AuthenticationControllerTests
{
    private readonly Mock<IAuthenticationService> _mockAuthService;
    private readonly Mock<IUserService> _mockUserService;
    private readonly Mock<IHttpContextAccessor> _mockHttpContextAccessor;
    private readonly AuthenticationController _controller;

    public AuthenticationControllerTests()
    {
        _mockAuthService = new Mock<IAuthenticationService>();
        _mockUserService = new Mock<IUserService>();
        _mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
        _controller = new AuthenticationController(_mockAuthService.Object, _mockUserService.Object, _mockHttpContextAccessor.Object);
    }

    [Fact]
    public void Register_UsernameExists_ReturnsBadRequest()
    {
        // Arrange
        RegisterUserRequest request = new() { UserName = "existingUser", Email = "test@example.com", Password = "password123" };
        _mockUserService.Setup(x => x.CheckIfUsernameExists(request.UserName)).Returns(true);

        // Act
        IActionResult result = _controller.Register(request);

        // Assert
        BadRequestObjectResult badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Username already in use.", badRequestResult.Value);
    }

    [Fact]
    public void Register_ValidRequest_ReturnsOk()
    {
        // Arrange
        RegisterUserRequest request = new() { UserName = "newUser", Email = "test@example.com", Password = "password123" };
        _mockUserService.Setup(x => x.CheckIfUsernameExists(request.UserName)).Returns(false);
        _mockUserService.Setup(x => x.CreateUser(It.IsAny<User>())).Returns(true);

        // Act
        IActionResult result = _controller.Register(request);

        // Assert
        Assert.IsType<OkResult>(result);
    }

    [Fact]
    public void Login_InvalidUsername_ReturnsBadRequest()
    {
        // Arrange
        AccessRequest request = new() { UserName = "nonExistingUser", Password = "password123" };
        _mockUserService.Setup(x => x.GetUserByName(request.UserName)).Returns((User) null);

        // Act
        IActionResult result = _controller.Login(request);

        // Assert
        BadRequestObjectResult badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Invalid credentials.", badRequestResult.Value);
    }

    [Fact]
    public void Login_InvalidPassword_ReturnsBadRequest()
    {
        // Arrange
        AccessRequest request = new() { UserName = "existingUser", Password = "wrongPassword" };
        User user = new() { UserName = "existingUser", Password = Encrypt.Hash("correctPassword") };
        _mockUserService.Setup(x => x.GetUserByName(request.UserName)).Returns(user);

        // Act
        IActionResult result = _controller.Login(request);

        // Assert
        BadRequestObjectResult badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Invalid credentials.", badRequestResult.Value);
    }

    [Fact]
    public void Login_ValidRequest_ReturnsOk()
    {
        // Arrange
        AccessRequest request = new() { UserName = "existingUser", Password = "password123" };
        User user = new() { UserName = "existingUser", Password = Encrypt.Hash("password123") };
        AuthenticationResponse authResponse = new("existingUser", "fake-jwt-token");
        _mockUserService.Setup(x => x.GetUserByName(request.UserName)).Returns(user);
        _mockAuthService.Setup(x => x.GenerateJwt(user)).Returns(authResponse);

        // Act
        IActionResult result = _controller.Login(request);

        // Assert
        OkObjectResult okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(authResponse, okResult.Value);
    }
}