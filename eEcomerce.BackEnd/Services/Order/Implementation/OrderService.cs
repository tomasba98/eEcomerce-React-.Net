using eEcomerce.BackEnd.Services.DataAccessLayer;

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
        _orderGenericService.InsertAsync(order);
        return order;
    }

    public Order? GetOrderById(Guid orderId)
    {
        return _orderGenericService.FilterByExpressionLinq(order => order.Id == orderId).FirstOrDefault();
    }

    public IEnumerable<Order> GetUserOrders(Guid? userId)
    {
        return _orderGenericService.FilterByExpressionLinq(order => order.UserId == userId).ToList();
    }
}
