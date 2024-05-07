
namespace eEcomerce.BackEnd.Services.Product;

using eEcomerce.BackEnd.Entities.Product;

public interface IProductService
{
    Product CreateProduct(Product product);

    Product? GetProductById(int productId);

    Product? GetProductByName(string productName);

    Product? GetProductByBrand(string productBrand);

    Task<bool> UpdateProduct(Product product);

    Task<bool> DeleteProduct(Product product);

    IEnumerable<Product> GetAllProducts();

    IEnumerable<Product> GetUserProducts(int userId);

}
