
namespace eEcomerce.BackEnd.Models.OrderProduct;

using eEcomerce.BackEnd.Models.Product;

public class OrderProductResponse
{
    public required int Quantity { get; set; }

    public required decimal ProductValue { get; set; }

    public required ProductResponse Proudct { get; set; }
}
