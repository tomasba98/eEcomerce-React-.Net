
namespace eEcomerce.BackEnd.Services.OrderProduct;

using eEcomerce.BackEnd.Entities.OrderProduct;

/// <summary>
/// Service interface for managing order products.
/// </summary>
public interface IOrderProductService
{

    /// <summary>
    /// Creates a new order product.
    /// </summary>
    /// <param name="orderProduct">The order product to create.</param>
    /// <returns>The created order product.</returns>
    OrderProduct CreateOrderProduct(OrderProduct orderProduct);

    /// <summary>
    /// Retrieves an order product by its unique identifier.
    /// </summary>
    /// <param name="orderProductId">The unique identifier of the order product.</param>
    /// <returns>The order product if found; otherwise, null.</returns>
    OrderProduct? GetOrderProductById(Guid orderProductId);

    /// <summary>
    /// Retrieves all order products associated with a specific order.
    /// </summary>
    /// <param name="orderId">The unique identifier of the order.</param>
    /// <returns>A collection of order products associated with the order.</returns>
    IEnumerable<OrderProduct> GetOrderProducts_ByOrderId(Guid orderId);
}
