namespace eEcomerce.BackEnd.Models.Order;
public class OrderResponse
{
    public required Guid Id { get; set; }
    public required DateTime DatePurchase { get; set; }
    public required decimal TotalPrice { get; set; }

}
