using eEcomerce.BackEnd.Entities.Comment;
using eEcomerce.BackEnd.Entities.Product;
using eEcomerce.BackEnd.Entities.User;
using eEcomerce.BackEnd.Models.Comment;
using eEcomerce.BackEnd.Services.Comment;
using eEcomerce.BackEnd.Services.Product;
using eEcomerce.BackEnd.Services.User;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eEcomerce.BackEnd.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CommentController : BaseController
    {
        private readonly ICommentService _commentService;
        private readonly IProductService _productService;

        public CommentController(
            ICommentService commentService,
            IHttpContextAccessor httpContextAccessor,
            IUserService userService,
            IProductService productService)
            : base(httpContextAccessor, userService)
        {
            _commentService = commentService;
            _productService = productService;
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
        public ActionResult<CommentResponse> CreateComment(CommentRequest commentRequest, Guid productId)
        {
            Guid? userId = GetUserIdFromToken();

            if (!ValidateUserId(userId))
            {
                return BadRequest("Invalid user ID.");
            }

            Guid value = userId!.Value;
            User? user = _userService.GetUserById(value);
            Product? product = _productService.GetProductById(productId);

            if (product == null)
            {
                return BadRequest("Product not found.");
            }

            if (user == null)
            {
                return BadRequest("User not found.");
            }

            Comment comment = new(commentRequest.Text, product, user);
            Comment createdComment = _commentService.CreateComment(comment);

            if (createdComment is null)
            {
                return BadRequest("Can't create the comment.");
            }

            CommentResponse commentResponse = MapToDto(createdComment);

            return Ok(commentResponse);
        }

        [HttpGet("{productId}")]
        public ActionResult<IEnumerable<CommentResponse>> GetProductComments(Guid productId)
        {
            IEnumerable<Comment> comments = _commentService.GetCommentsByProduct(productId);
            IEnumerable<CommentResponse> result = comments.Select(MapToDto).ToList();

            return Ok(result);
        }

        [HttpGet("users")]
        [Authorize]
        public ActionResult<IEnumerable<CommentResponse>> GetUserComments()
        {
            Guid? userId = GetUserIdFromToken();

            if (!ValidateUserId(userId))
            {
                return BadRequest("Invalid user ID.");
            }

            IEnumerable<Comment> comments = _commentService.GetCommentsByUser(userId!.Value);
            IEnumerable<CommentResponse> result = comments.Select(MapToDto).ToList();

            return Ok(result);
        }

        [HttpDelete("users")]
        [Authorize]
        public async Task<ActionResult> DeleteComment(Guid commentId)
        {
            Guid? userId = GetUserIdFromToken();

            if (!ValidateUserId(userId))
            {
                return BadRequest("Invalid user ID.");
            }

            User? user = _userService.GetUserById(userId!.Value);
            Comment? comment = _commentService.GetCommentById(commentId);

            if (user == null || comment == null || comment.User.Id != user.Id)
            {
                return BadRequest("Comment not found or user mismatch.");
            }

            bool result = await _commentService.DeleteComment(comment);
            if (!result)
            {
                return BadRequest("Failed to delete the comment.");
            }

            return Ok();
        }
    }
}
