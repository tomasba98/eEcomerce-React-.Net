namespace eEcomerce.BackEnd.Entities.Comment;

using eEcomerce.BackEnd.Entities.Product;
using eEcomerce.BackEnd.Entities.User;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("Comments")]
public class Comment : EntityBase
{
    public Comment() { }

    public Comment(string text, Product product, User user)
    {
        Text = text;
        PostedAt = DateTime.UtcNow;
        Product = product;
        ProductId = product.Id;
        User = user;
        UserId = user.Id;
    }

    public string Text { get; set; } = null!;
    public DateTime PostedAt { get; set; }

    [Range(1, 5, ErrorMessage = "El rating debe estar entre 1 y 5.")]
    public float Rating { get; set; }

    [Required]
    [Column("ProductId")]
    public Product Product { get; set; } = null!;
    public Guid ProductId { get; set; }

    [Required]
    [Column("UserId")]
    public User User { get; set; } = null!;
    public Guid UserId { get; set; }
}
