
namespace eEcomerce.BackEnd.Models.OrderProduct;

using eEcomerce.BackEnd.Models.Product;

public class OrderProductResponse
{
    public int Quantity { get; set; }

    public decimal ProductValue { get; set; }

    public ProductResponse proudct { get; set; }
}
