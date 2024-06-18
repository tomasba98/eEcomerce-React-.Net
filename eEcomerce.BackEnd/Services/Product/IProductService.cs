namespace eEcomerce.BackEnd.Services.Product;

using eEcomerce.BackEnd.Entities.Product;

/// <summary>
/// Service interface for managing products.
/// </summary>
public interface IProductService
{
    /// <summary>
    /// Creates a new product.
    /// </summary>
    /// <param name="product">The product to create.</param>
    /// <returns>The created product.</returns>
    Product CreateProduct(Product product);

    /// <summary>
    /// Retrieves a product by its unique identifier.
    /// </summary>
    /// <param name="productId">The unique identifier of the product.</param>
    /// <returns>The product if found; otherwise, null.</returns>
    Product? GetProductById(Guid productId);

    /// <summary>
    /// Retrieves a product by its name.
    /// </summary>
    /// <param name="productName">The name of the product.</param>
    /// <returns>The product if found; otherwise, null.</returns>
    Product? GetProductByName(string productName);

    /// <summary>
    /// Retrieves a product by its brand.
    /// </summary>
    /// <param name="productBrand">The brand of the product.</param>
    /// <returns>The product if found; otherwise, null.</returns>
    Product? GetProductByBrand(string productBrand);

    /// <summary>
    /// Updates an existing product asynchronously.
    /// </summary>
    /// <param name="product">The product to update.</param>
    /// <returns>True if the product was updated successfully; otherwise, false.</returns>
    Task<bool> UpdateProduct(Product product);

    /// <summary>
    /// Deletes an existing product asynchronously.
    /// </summary>
    /// <param name="product">The product to delete.</param>
    /// <returns>True if the product was deleted successfully; otherwise, false.</returns>
    Task<bool> DeleteProduct(Product product);

    /// <summary>
    /// Retrieves all products.
    /// </summary>
    /// <returns>A collection of all products.</returns>
    IEnumerable<Product> GetAllProducts();

    /// <summary>
    /// Retrieves all products owned by a specific user.
    /// </summary>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <returns>A collection of products owned by the user.</returns>
    IEnumerable<Product> GetUserProducts(Guid userId);

}
