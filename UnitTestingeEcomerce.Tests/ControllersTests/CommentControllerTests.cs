using eEcomerce.BackEnd.Controllers;
using eEcomerce.BackEnd.Entities.Comment;
using eEcomerce.BackEnd.Entities.Product;
using eEcomerce.BackEnd.Entities.User;
using eEcomerce.BackEnd.Models.Comment;
using eEcomerce.BackEnd.Services.Comment;
using eEcomerce.BackEnd.Services.Product;
using eEcomerce.BackEnd.Services.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Moq;

using UnitTestingeEcomerce.Tests.Utils;

namespace UnitTestingeEcomerce.Tests.ControllersTests;

public class CommentControllerTests
{
    private readonly Mock<ICommentService> _mockCommentService;
    private readonly Mock<IProductService> _mockProductService;
    private readonly Mock<IUserService> _mockUserService;
    private readonly Mock<IHttpContextAccessor> _mockHttpContextAccessor;
    private readonly CommentController _controller;
    private readonly UserTesterBuilder _userBuilder;

    public CommentControllerTests()
    {
        _mockCommentService = new Mock<ICommentService>();
        _mockProductService = new Mock<IProductService>();
        _mockUserService = new Mock<IUserService>();
        _mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
        _controller = new CommentController(
            _mockCommentService.Object,
            _mockHttpContextAccessor.Object,
            _mockUserService.Object,
            _mockProductService.Object
        );
        _userBuilder = new UserTesterBuilder(_mockHttpContextAccessor);
    }

    [Fact]
    public void CreateComment_InvalidUserId_ReturnsBadRequest()
    {
        // Arrange
        CommentRequest commentRequest = new() { Text = "Sample comment", Rating = 0 };
        Guid productId = Guid.NewGuid();
        _userBuilder.WithId(Guid.Empty).SetupUserInHttpContext(); // Invalid user

        // Act
        ActionResult<CommentResponse> result = _controller.CreateComment(commentRequest, productId);

        // Assert
        BadRequestObjectResult badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
        Assert.Equal("Invalid user ID.", badRequestResult.Value);
    }

    [Fact]
    public void CreateComment_ProductNotFound_ReturnsBadRequest()
    {
        // Arrange
        Guid userId = Guid.NewGuid();
        _userBuilder.WithId(userId).SetupUserInHttpContext();

        CommentRequest commentRequest = new() { Text = "Sample comment", Rating = 0 };
        Guid productId = Guid.NewGuid();

        _mockUserService.Setup(x => x.GetUserById(userId)).Returns(new User());
        _mockProductService.Setup(x => x.GetProductById(productId)).Returns((Product) null);

        // Act
        ActionResult<CommentResponse> result = _controller.CreateComment(commentRequest, productId);

        // Assert
        BadRequestObjectResult badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
        Assert.Equal("Product not found.", badRequestResult.Value);
    }

    [Fact]
    public void CreateComment_Success_ReturnsCommentResponse()
    {
        // Arrange
        Guid userId = Guid.NewGuid();
        _userBuilder.WithId(userId).WithUserName("testuser").WithPassword("password123").WithEmail("test@example.com").SetupUserInHttpContext();

        CommentRequest commentRequest = new() { Text = "Sample comment", Rating = 0 };
        Guid productId = Guid.NewGuid();
        User user = _userBuilder.Build();
        Product product = new();
        Comment comment = new("Sample comment", product, user);
        Comment createdComment = new("Sample comment", product, user) { Id = Guid.NewGuid() };

        _mockUserService.Setup(x => x.GetUserById(userId)).Returns(user);
        _mockProductService.Setup(x => x.GetProductById(productId)).Returns(product);
        _mockCommentService.Setup(x => x.CreateComment(It.IsAny<Comment>())).Returns(createdComment);

        // Act
        ActionResult<CommentResponse> result = _controller.CreateComment(commentRequest, productId);

        // Assert
        OkObjectResult okResult = Assert.IsType<OkObjectResult>(result.Result);
        CommentResponse returnValue = Assert.IsType<CommentResponse>(okResult.Value);

        Assert.Equal(createdComment.Id, returnValue.CommentId);
        Assert.Equal(createdComment.Text, returnValue.Text);
        Assert.Equal(createdComment.PostedAt, returnValue.PostedAt);
    }

    [Fact]
    public void GetProductComments_ReturnsListOfComments()
    {
        // Arrange
        Guid productId = Guid.NewGuid();
        List<Comment> comments = new()
        {
            new Comment { Id = Guid.NewGuid(), Text = "Comment 1", PostedAt = DateTime.UtcNow },
            new Comment { Id = Guid.NewGuid(), Text = "Comment 2", PostedAt = DateTime.UtcNow }
        };

        _mockCommentService.Setup(x => x.GetCommentsByProduct(productId)).Returns(comments);

        // Act
        ActionResult<IEnumerable<CommentResponse>> result = _controller.GetProductComments(productId);

        // Assert
        OkObjectResult okResult = Assert.IsType<OkObjectResult>(result.Result);
        List<CommentResponse> returnValue = Assert.IsType<List<CommentResponse>>(okResult.Value);

        Assert.Equal(2, returnValue.Count);
        Assert.Equal("Comment 1", returnValue[0].Text);
        Assert.Equal("Comment 2", returnValue[1].Text);
    }

    [Fact]
    public void GetUserComments_ReturnsListOfUserComments()
    {
        // Arrange
        Guid userId = Guid.NewGuid();
        _userBuilder.WithId(userId).SetupUserInHttpContext();

        List<Comment> comments = new()
        {
            new Comment { Id = Guid.NewGuid(), Text = "User Comment 1", PostedAt = DateTime.UtcNow},
            new Comment { Id = Guid.NewGuid(), Text = "User Comment 2", PostedAt = DateTime.UtcNow}
        };

        _mockCommentService.Setup(x => x.GetCommentsByUser(userId)).Returns(comments);
        _mockUserService.Setup(x => x.GetUserById(userId)).Returns(new User());

        // Act
        ActionResult<IEnumerable<CommentResponse>> result = _controller.GetUserComments();

        // Assert
        OkObjectResult okResult = Assert.IsType<OkObjectResult>(result.Result);
        List<CommentResponse> returnValue = Assert.IsType<List<CommentResponse>>(okResult.Value);

        Assert.Equal(2, returnValue.Count);
        Assert.Equal("User Comment 1", returnValue[0].Text);
        Assert.Equal("User Comment 2", returnValue[1].Text);
    }

    [Fact]
    public async Task DeleteComment_InvalidUserId_ReturnsBadRequest()
    {
        // Arrange
        Guid commentId = Guid.NewGuid();
        _userBuilder.WithId(Guid.Empty).SetupUserInHttpContext(); // Invalid user

        // Act
        ActionResult result = await _controller.DeleteComment(commentId);

        // Assert
        BadRequestObjectResult badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Invalid user ID.", badRequestResult.Value);
    }

    [Fact]
    public async Task DeleteComment_CommentNotFound_ReturnsBadRequest()
    {
        // Arrange
        Guid userId = Guid.NewGuid();
        _userBuilder.WithId(userId).SetupUserInHttpContext();

        Guid commentId = Guid.NewGuid();
        User user = new() { Id = userId };

        _mockUserService.Setup(x => x.GetUserById(userId)).Returns(user);
        _mockCommentService.Setup(x => x.GetCommentById(commentId)).Returns((Comment) null);

        // Act
        ActionResult result = await _controller.DeleteComment(commentId);

        // Assert
        BadRequestObjectResult badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Comment not found or user mismatch.", badRequestResult.Value);
    }

    [Fact]
    public async Task DeleteComment_Success_ReturnsOk()
    {
        // Arrange
        Guid userId = Guid.NewGuid();
        _userBuilder.WithId(userId).SetupUserInHttpContext();

        Guid commentId = Guid.NewGuid();
        User user = new() { Id = userId };
        Comment comment = new() { Id = commentId, User = user };

        _mockUserService.Setup(x => x.GetUserById(userId)).Returns(user);
        _mockCommentService.Setup(x => x.GetCommentById(commentId)).Returns(comment);
        _mockCommentService.Setup(x => x.DeleteComment(comment)).ReturnsAsync(true);

        // Act
        ActionResult result = await _controller.DeleteComment(commentId);

        // Assert
        OkResult okResult = Assert.IsType<OkResult>(result);
    }
}