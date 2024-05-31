
namespace eEcomerce.BackEnd.Services.OrderProduct;

using eEcomerce.BackEnd.Entities.OrderProduct;

public interface IOrderProductService
{

    OrderProduct CreateOrderProduct(OrderProduct orderProduct);

    OrderProduct? GetOrderProductById(Guid orderProductId);

    IEnumerable<OrderProduct> GetOrderProducts_ByOrderId(Guid orderId);
}
