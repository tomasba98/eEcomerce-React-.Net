namespace eEcomerce.BackEnd.Models.Product;

public class ProductRequest
{
    public required string Name { get; set; }

    public required string Description { get; set; }

    public required decimal Price { get; set; }

    public required string Brand { get; set; }
}
