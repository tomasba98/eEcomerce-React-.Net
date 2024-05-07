using eEcomerce.BackEnd.Entities.Category;
using eEcomerce.BackEnd.Entities.Order;
using eEcomerce.BackEnd.Entities.OrderProduct;
using eEcomerce.BackEnd.Entities.Product;
using eEcomerce.BackEnd.Entities.ProductComment;
using eEcomerce.BackEnd.Entities.User;

using Microsoft.EntityFrameworkCore;
namespace eEcomerce.BackEnd.Data
{
    /// <summary>
    /// Represents the application's database context.
    /// </summary>
    public class AppDbContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AppDbContext"/> class.
        /// </summary>
        /// <param name="options">The options to be used by the context.</param>
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public virtual DbSet<User> Users { get; set; }

        public virtual DbSet<Order> Orders { get; set; }

        public virtual DbSet<Product> Products { get; set; }

        public virtual DbSet<Category> Categorys { get; set; }

        public virtual DbSet<OrderProduct> OrderProducts { get; set; }

        public virtual DbSet<ProductComment> ProductComment { get; set; }
    }
}
