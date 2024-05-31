
namespace eEcomerce.BackEnd.Services.Order;

using eEcomerce.BackEnd.Entities.Order;
public interface IOrderService
{
    public Order CreateOrder(Order order);

    Order? GetOrderById(Guid productId);

    IEnumerable<Order> GetUserOrders(Guid? userId);
}
