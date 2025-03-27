namespace eEcomerce.BackEnd.Services.Comment;

using eEcomerce.BackEnd.Entities.Comment;

/// <summary>
/// Service interface for managing comments.
/// </summary>
public interface ICommentService
{
    /// <summary>
    /// Creates a new comment.
    /// </summary>
    /// <param name="comment">The comment to create.</param>
    /// <returns>The created comment.</returns>
    Comment CreateComment(Comment comment);

    /// <summary>
    /// Retrieves a comment by its unique identifier.
    /// </summary>
    /// <param name="commentId">The unique identifier of the comment.</param>
    /// <returns>The comment if found; otherwise, null.</returns>
    Comment? GetCommentById(Guid commentId);

    /// <summary>
    /// Deletes a comment asynchronously.
    /// </summary>
    /// <param name="comment">The comment to delete.</param>
    /// <returns>True if the comment was deleted successfully; otherwise, false.</returns>
    Task<bool> DeleteComment(Comment comment);

    /// <summary>
    /// Retrieve the rating of a specific product.
    /// </summary>
    /// <param name="productId">The unique identifier of the product.</param>
    /// <returns>Return the promedy rating of the product.</returns>
    public float GetProductRating(Guid productId);

    /// <summary>
    /// Retrieves all comments associated with a specific product.
    /// </summary>
    /// <param name="productId">The unique identifier of the product.</param>
    /// <returns>A collection of comments associated with the product.</returns>
    IEnumerable<Comment> GetCommentsByProduct(Guid productId);

    /// <summary>
    /// Retrieves all comments posted by a specific user.
    /// </summary>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <returns>A collection of comments posted by the user.</returns>
    IEnumerable<Comment> GetCommentsByUser(Guid userId);
}

