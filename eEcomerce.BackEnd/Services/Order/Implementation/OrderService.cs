using eEcomerce.BackEnd.Services.DataAccessLayer.IGenericService;

namespace eEcomerce.BackEnd.Services.Order.Implementation;

using eEcomerce.BackEnd.Entities.Order;

public class OrderService : IOrderService
{

    private readonly IGenericService<Order> _orderGenericService;

    public OrderService(IGenericService<Order> orederGenericService)
    {
        _orderGenericService = orederGenericService;
    }
    public Order CreateOrder(Order order)
    {
        _orderGenericService.Insert(order);
        return order;
    }

    public Order? GetOrderById(int orderId)
    {
        return _orderGenericService.FilterByExpression(order => order.Id == orderId).FirstOrDefault();
    }

    public IEnumerable<Order> GetUserOrders(int? userId)
    {
        return _orderGenericService.FilterByExpression(order => order.UserId == userId).ToList();
    }
}
