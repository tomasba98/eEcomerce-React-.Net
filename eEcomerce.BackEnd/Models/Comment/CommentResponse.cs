namespace eEcomerce.BackEnd.Models.Comment;

public class CommentResponse
{
    public CommentResponse()
    {
    }
    public CommentResponse(Guid commentId, string text, DateTime datePurchase)
    {
        CommentId = commentId;
        Text = text;
        PostedAt = datePurchase;
    }

    public Guid CommentId { get; set; }
    public string Text { get; set; } = null!;

    public DateTime PostedAt { get; set; }
}
