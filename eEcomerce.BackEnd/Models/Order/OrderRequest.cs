namespace eEcomerce.BackEnd.Models.Order;

using eEcomerce.BackEnd.Models.OrderProduct;
public class OrderRequest
{
    public ICollection<OrderProductRequest> ListOrderProducts { get; set; } = new List<OrderProductRequest>();
}
