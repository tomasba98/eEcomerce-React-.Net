namespace eEcomerce.BackEnd.Models.Product;

public class ProductRequest
{
    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public decimal Price { get; set; }

    public string Brand { get; set; } = string.Empty;
}
