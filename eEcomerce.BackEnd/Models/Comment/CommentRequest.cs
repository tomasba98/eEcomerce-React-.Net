namespace eEcomerce.BackEnd.Models.Comment;

public class CommentRequest
{
    public required string Text { get; set; } = null!;

    public required float Rating { get; set; }
}
