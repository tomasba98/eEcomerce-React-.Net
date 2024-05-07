namespace eEcomerce.BackEnd.Entities.ProductComment;

using eEcomerce.BackEnd.Entities.Product;
using eEcomerce.BackEnd.Entities.User;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("ProductComments")]
public class ProductComment : EntityBase
{
    public string Text { get; set; } = null!;
    public DateTime PostedAt { get; set; }

    [Required]
    [Column("ProductId")]
    public Product Product { get; set; } = null!;
    public int ProductId { get; set; }

    [Required]
    [Column("UserId")]
    public User User { get; set; } = null!;
    public int UserId { get; set; }
}
