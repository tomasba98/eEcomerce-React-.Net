
namespace eEcomerce.BackEnd.Services.OrderProduct.Implementation;

using eEcomerce.BackEnd.Entities.OrderProduct;
using eEcomerce.BackEnd.Services.DataAccessLayer.IGenericService;

public class OrderProductService : IOrderProductService
{
    private readonly IGenericService<OrderProduct> _orderProductGenericService;

    public OrderProductService(IGenericService<OrderProduct> orderProductGenericService)
    {
        _orderProductGenericService = orderProductGenericService;
    }

    public OrderProduct CreateOrderProduct(OrderProduct orderProduct)
    {
        _orderProductGenericService.Insert(orderProduct);
        return orderProduct;
    }

    public OrderProduct? GetOrderProductById(Guid orderProductId)
    {
        return _orderProductGenericService.FilterByExpression(op => op.Id == orderProductId).FirstOrDefault();
    }

    public IEnumerable<OrderProduct> GetOrderProducts_ByOrderId(Guid orderId)
    {
        return _orderProductGenericService.FilterByExpression(op => op.OrderId == orderId).ToList();
    }

}
