namespace eEcomerce.BackEnd.Models.Product;

public class ProductResponse
{
    public int Id { get; set; }
    public string Name { get; set; }

    public string Description { get; set; }

    public decimal Price { get; set; }

    public string Brand { get; set; }

    public int CategoryId { get; set; }
}
