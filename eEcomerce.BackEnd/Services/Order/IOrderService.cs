
namespace eEcomerce.BackEnd.Services.Order;

using eEcomerce.BackEnd.Entities.Order;

/// <summary>
/// Service interface for managing orders.
/// </summary>
public interface IOrderService
{
    /// <summary>
    /// Creates a new order.
    /// </summary>
    /// <param name="order">The order to create.</param>
    /// <returns>The created order.</returns>
    Order CreateOrder(Order order);

    /// <summary>
    /// Retrieves an order by its unique identifier.
    /// </summary>
    /// <param name="orderId">The unique identifier of the order.</param>
    /// <returns>The order if found; otherwise, null.</returns>
    Order? GetOrderById(Guid orderId);

    /// <summary>
    /// Retrieves all orders placed by a specific user.
    /// </summary>
    /// <param name="userId">The unique identifier of the user. If null, retrieves orders for all users.</param>
    /// <returns>A collection of orders placed by the user.</returns>
    IEnumerable<Order> GetUserOrders(Guid? userId);
}
