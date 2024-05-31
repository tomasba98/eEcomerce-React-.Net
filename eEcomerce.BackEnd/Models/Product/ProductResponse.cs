namespace eEcomerce.BackEnd.Models.Product;

public class ProductResponse
{
    public Guid Id { get; set; }
    public required string Name { get; set; }

    public required string Description { get; set; }

    public required decimal Price { get; set; }

    public required string Brand { get; set; }

    public Guid CategoryId { get; set; }
}
