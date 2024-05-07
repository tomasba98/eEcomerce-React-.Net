
namespace eEcomerce.BackEnd.Services.OrderProduct;

using eEcomerce.BackEnd.Entities.OrderProduct;

public interface IOrderProductService
{

    OrderProduct CreateOrderProduct(OrderProduct orderProduct);

    OrderProduct? GetOrderProductById(int orderProductId);

    IEnumerable<OrderProduct> GetOrderProducts_ByOrderId(int orderId);
}
