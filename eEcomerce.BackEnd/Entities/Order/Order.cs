using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eEcomerce.BackEnd.Entities.Order;

using eEcomerce.BackEnd.Entities.OrderProduct;
using eEcomerce.BackEnd.Entities.User;

[Table("Orders")]
public class Order : EntityBase
{
    public Order() { }

    public Order(decimal totalPrice, User user, int totalQuantityProducts)
    {
        DatePurchase = DateTime.UtcNow;
        User = user;
        UserId = user.Id;
        TotalPrice = totalPrice;
        TotalQuantityProducts = totalQuantityProducts;
    }

    [Required]
    public DateTime DatePurchase { get; set; }

    [Required]
    public decimal TotalPrice { get; set; }

    [Required]
    public int TotalQuantityProducts { get; set; }

    public virtual ICollection<OrderProduct> OrderProducts { get; set; } = [];

    [Required]
    [Column("UserId")]
    public User? User { get; set; }
    public int UserId { get; set; }
}
