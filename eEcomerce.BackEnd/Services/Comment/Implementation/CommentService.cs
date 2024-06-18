
using eEcomerce.BackEnd.Services.DataAccessLayer;

namespace eEcomerce.BackEnd.Services.Comment.Implementation;

using eEcomerce.BackEnd.Entities.Comment;

public class CommentService : ICommentService
{
    private readonly IGenericService<Comment> _commentGenericService;

    public CommentService(IGenericService<Comment> commentGenericService)
    {
        _commentGenericService = commentGenericService;
    }

    public Comment CreateComment(Comment comment)
    {
        _commentGenericService.InsertAsync(comment);
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
        return _commentGenericService.FilterByExpressionLinq(comment => comment.Id == commentId).FirstOrDefault();
    }

    public IEnumerable<Comment> GetCommentsByProduct(Guid productId)
    {
        return _commentGenericService.FilterByExpressionLinq(comment => comment.ProductId == productId).ToList();
    }

    public IEnumerable<Comment> GetCommentsByUser(Guid userId)
    {
        return _commentGenericService.FilterByExpressionLinq(comment => comment.UserId == userId).ToList();
    }
}
