namespace eEcomerce.BackEnd.Models.Category;

public class CategoryModel
{
    public required Guid Id { get; set; }
    public required char Letter { get; set; }
    public required string Name { get; set; }
}

