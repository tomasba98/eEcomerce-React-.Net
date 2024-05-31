using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eEcomerce.BackEnd.Entities.Product;

using eEcomerce.BackEnd.Entities.Category;
using eEcomerce.BackEnd.Entities.OrderProduct;
using eEcomerce.BackEnd.Entities.User;

[Table("Products")]
public class Product : EntityBase
{
    public Product()
    {

    }
    public Product(string name, string description, decimal price, string brand, Category category, User user)
    {
        Name = name;
        Description = description;
        Price = price;
        Brand = brand;
        Category = category;
        CategoryId = category.Id;
        User = user;
        UserId = user.Id;
    }

    [Required]
    public string Name { get; set; }

    [Required]
    public string Description { get; set; }

    [Required]
    public decimal Price { get; set; }

    [Required]
    public string Brand { get; set; }

    // Collection of OrderProducts associated with this Product
    public virtual ICollection<OrderProduct> OrderProducts { get; set; } = [];

    [Required]
    [Column("CategoryId")]
    public virtual Category Category { get; set; }

    public Guid CategoryId { get; set; }

    // Navigation property for the User
    [Column("UserId")]
    public virtual User User { get; set; }

    public Guid UserId { get; set; }
}
