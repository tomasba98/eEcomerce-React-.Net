using eEcomerce.BackEnd.Entities.Product;
using eEcomerce.BackEnd.Entities.User;
using eEcomerce.BackEnd.Models.Comment;
using eEcomerce.BackEnd.Services.Comment;
using eEcomerce.BackEnd.Services.Product;
using eEcomerce.BackEnd.Services.Users;

using Microsoft.AspNetCore.Mvc;

using System.Security.Claims;

namespace eEcomerce.BackEnd.Controllers;

using eEcomerce.BackEnd.Entities.Comment;
using eEcomerce.BackEnd.Models.Product;

using Microsoft.AspNetCore.Authorization;
using Microsoft.CodeAnalysis;

[ApiController]
[Route("api/[controller]")]
public class CommentController : ControllerBase
{
    private readonly ICommentService _commentService;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IUserService _userService;
    private readonly IProductService _productService;

    public CommentController(ICommentService commentService, IHttpContextAccessor httpContextAccessor, IUserService userService, IProductService productService)
    {
        _commentService = commentService;
        _httpContextAccessor = httpContextAccessor;
        _userService = userService;
        _productService = productService;
    }

    private Guid? GetUserIdFromToken()
    {
        Claim? userIdClaim = _httpContextAccessor.HttpContext.User.FindFirst("UserId");
        if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out Guid userId))
        {
            return null;
        }
        return userId;
    }

    private bool ValidateUserId(Guid? userId)
    {
        if (userId is null || _userService.GetUserById((Guid) userId) is null)
        {
            return false;
        }
        return true;
    }

    private static CommentResponse MapToDto(Comment comment)
    {
        return new CommentResponse
        {
            CommentId = comment.Id,
            Text = comment.Text,
            PostedAt = comment.PostedAt,
        };
    }

    [HttpPost]
    [Authorize]
    public ActionResult<CommentRequest> CreateComment(CommentRequest commentRequest, Guid productId)
    {
        Guid? userId = GetUserIdFromToken();

        if (!ValidateUserId(userId))
        {
            return BadRequest();
        }
        User? user = _userService.GetUserById((Guid) userId);
        Product? product = _productService.GetProductById(productId);

        if (product is null)
        {
            return BadRequest();
        }

        Comment comment = new(commentRequest.Text, product, user);

        Comment createdComment = _commentService.CreateComment(comment);
        if (createdComment is null)
        {
            return BadRequest("Can't create the comment");
        }

        CommentResponse commentResponse = new(comment.Id, comment.Text, comment.PostedAt);

        return Ok(commentResponse);
    }

    [HttpGet("{productId}")]

    public ActionResult<IEnumerable<CommentResponse>> GetProductComments(Guid productId)
    {
        IEnumerable<Comment> comments = _commentService.GetCommentsByProduct(productId);
        IEnumerable<CommentResponse> result = comments.Select(c => MapToDto(c)).ToList();

        return Ok(result);
    }

    [HttpGet("user")]
    [Authorize]
    public ActionResult<IEnumerable<CommentResponse>> GetUserComments()
    {
        Guid? userId = GetUserIdFromToken();

        if (!ValidateUserId(userId))
        {
            return BadRequest();
        }

        IEnumerable<Comment> comments = _commentService.GetCommentsByUser((Guid) userId);
        IEnumerable<CommentResponse> result = comments.Select(c => MapToDto(c)).ToList();

        return Ok(result);
    }

    [HttpDelete("user")]
    [Authorize]
    public async Task<ActionResult<ProductResponse>> DeleteComment(Guid commentId)
    {
        Guid? userId = GetUserIdFromToken();

        if (!ValidateUserId(userId))
        {
            return BadRequest();
        }
        User? user = _userService.GetUserById((Guid) userId);
        Comment? comment = _commentService.GetCommentById(commentId);

        if (user is null || comment is null || comment.User.Id != user.Id)
        {
            return BadRequest();
        }

        bool result = await _commentService.DeleteComment(comment);
        if (!result)
        {
            return BadRequest();
        }

        return Ok();
    }
}