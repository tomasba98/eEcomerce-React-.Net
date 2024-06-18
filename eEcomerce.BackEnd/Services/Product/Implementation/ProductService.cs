using eEcomerce.BackEnd.Services.DataAccessLayer;

namespace eEcomerce.BackEnd.Services.Product.Implementation;

using eEcomerce.BackEnd.Entities.Product;
using eEcomerce.BackEnd.Services.Product;

using Microsoft.CodeAnalysis;

using System.Collections.Generic;
using System.Threading.Tasks;

public class ProductService : IProductService
{
    private readonly IGenericService<Product> _productGenericService;

    public ProductService(IGenericService<Product> productGenericService)
    {
        _productGenericService = productGenericService;
    }

    public Product CreateProduct(Product product)
    {
        _productGenericService.InsertAsync(product);
        return product;
    }

    public IEnumerable<Product> GetAllProducts()
    {
        IEnumerable<Product> products = _productGenericService.FindAllAsync().Result;
        return products.ToList();
    }

    public IEnumerable<Product> GetUserProducts(Guid userId)
    {
        return _productGenericService.FilterByExpressionLinq(product => product.UserId == userId).ToList();
    }
    public Product? GetProductById(Guid productId)
    {
        return _productGenericService.FilterByExpressionLinq(product => product.Id == productId).FirstOrDefault();
    }

    public Product? GetProductByBrand(string productBrand)
    {
        return _productGenericService.FilterByExpressionLinq(product => product.Brand == productBrand).FirstOrDefault();
    }


    public Product? GetProductByName(string productName)
    {
        return _productGenericService.FilterByExpressionLinq(product => product.Name == productName).FirstOrDefault();
    }

    public async Task<bool> UpdateProduct(Product product)
    {
        try
        {
            await _productGenericService.UpdateAsync(product);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> DeleteProduct(Product product)
    {
        try
        {
            await _productGenericService.DeleteAsync(product);
            return true;
        }
        catch
        {
            return false;
        }
    }
}
