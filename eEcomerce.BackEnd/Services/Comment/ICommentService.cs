namespace eEcomerce.BackEnd.Services.Comment;

using eEcomerce.BackEnd.Entities.Comment;
public interface ICommentService
{
    Comment CreateComment(Comment comment);

    Comment? GetCommentById(Guid commentId);

    Task<bool> DeleteComment(Comment comment);

    IEnumerable<Comment> GetCommentsByProduct(Guid productId);

    IEnumerable<Comment> GetCommentsByUser(Guid userId);
}
