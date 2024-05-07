
namespace eEcomerce.BackEnd.Services.Order;

using eEcomerce.BackEnd.Entities.Order;
public interface IOrderService
{
    public Order CreateOrder(Order order);

    Order? GetOrderById(int productId);

    IEnumerable<Order> GetUserOrders(int? userId);
}
