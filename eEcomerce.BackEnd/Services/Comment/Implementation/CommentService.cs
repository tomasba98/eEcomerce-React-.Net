
namespace eEcomerce.BackEnd.Services.Comment.Implementation;

using eEcomerce.BackEnd.Entities.Comment;
using eEcomerce.BackEnd.Services.DataAccessLayer.IGenericService;

public class CommentService : ICommentService
{
    private readonly IGenericService<Comment> _commentGenericService;

    public CommentService(IGenericService<Comment> commentGenericService)
    {
        _commentGenericService = commentGenericService;
    }

    public Comment CreateComment(Comment comment)
    {
        _commentGenericService.Insert(comment);
        return comment;
    }

    public async Task<bool> DeleteComment(Comment comment)
    {
        try
        {
            await _commentGenericService.DeleteAsync(comment);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public Comment? GetCommentById(Guid commentId)
    {
        return _commentGenericService.FilterByExpression(comment => comment.Id == commentId).FirstOrDefault();
    }

    public IEnumerable<Comment> GetCommentsByProduct(Guid productId)
    {
        return _commentGenericService.FilterByExpression(comment => comment.ProductId == productId).ToList();
    }

    public IEnumerable<Comment> GetCommentsByUser(Guid userId)
    {
        return _commentGenericService.FilterByExpression(comment => comment.UserId == userId).ToList();
    }
}
