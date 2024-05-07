using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eEcomerce.BackEnd.Entities.Order;

using eEcomerce.BackEnd.Entities.OrderProduct;
using eEcomerce.BackEnd.Entities.User;
using eEcomerce.BackEnd.Models.Order;

[Table("Orders")]
public class Order : EntityBase
{
    public Order() { }

    public Order(ICollection<OrderRequest> orderProducts, User user)
    {
        DatePurchase = DateTime.Now;
        User = user;
        UserId = user.Id;
        TotalPrice = orderProducts.Sum(op => op. * op.Quantity);
    }

    [Required]
    public DateTime DatePurchase { get; set; }

    [Required]
    public decimal TotalPrice { get; set; }

    public virtual ICollection<OrderProduct> OrderProducts { get; set; } = [];

    [Required]
    [Column("UserId")]
    public User? User { get; set; }
    public int UserId { get; set; }
}
