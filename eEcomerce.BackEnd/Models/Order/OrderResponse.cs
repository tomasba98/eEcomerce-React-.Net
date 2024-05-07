namespace eEcomerce.BackEnd.Models.Order;
public class OrderResponse
{
    public int Id { get; set; }
    public DateTime DatePurchase { get; set; }
    public decimal TotalPrice { get; set; }

}
