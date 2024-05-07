using eEcomerce.BackEnd.Models.Product;

namespace eEcomerce.BackEnd.Models.Order;

public class OrderRequest
{
    public ICollection<ProductResponse> OrderProducts { get; set; } = new List<ProductResponse>();
}
