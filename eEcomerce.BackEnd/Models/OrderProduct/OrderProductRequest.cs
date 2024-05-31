namespace eEcomerce.BackEnd.Models.OrderProduct;

public class OrderProductRequest
{
    public required int Quantity { get; set; }
    public required Guid ProductId { get; set; }
}
