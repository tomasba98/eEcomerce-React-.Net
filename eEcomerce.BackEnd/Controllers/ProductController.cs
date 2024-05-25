using eEcomerce.BackEnd.Entities.Category;
using eEcomerce.BackEnd.Entities.Product;
using eEcomerce.BackEnd.Entities.User;
using eEcomerce.BackEnd.Models.Product;
using eEcomerce.BackEnd.Services.Category;
using eEcomerce.BackEnd.Services.Product;
using eEcomerce.BackEnd.Services.Users;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using System.Security.Claims;

namespace eEcomerce.BackEnd.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductController : ControllerBase
{
    private readonly IProductService _productService;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IUserService _userService;
    private readonly ICategoryService _categoryService;
    public ProductController(IProductService prudctService, IHttpContextAccessor httpContextAccessor, IUserService userService, ICategoryService categoryService)
    {
        _httpContextAccessor = httpContextAccessor;
        _productService = prudctService;
        _userService = userService;
        _categoryService = categoryService;
    }

    private int? GetUserIdFromToken()
    {
        Claim userIdClaim = _httpContextAccessor.HttpContext.User.FindFirst("UserId");
        if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
        {
            return null;
        }
        return userId;
    }

    private bool ValidateUserId(int userId)
    {
        if (_userService.GetUserById(userId) == null)
        {
            return false;
        }
        return true;
    }

    private static ProductResponse MapToDto(Product product)
    {
        return new ProductResponse
        {
            Id = product.Id,
            Name = product.Name,
            Brand = product.Brand,
            CategoryId = product.CategoryId,
            Price = product.Price,
            Description = product.Description
        };
    }

    [HttpGet]
    public ActionResult<IEnumerable<ProductResponse>> GetProductsList()
    {
        IEnumerable<Product> products = _productService.GetAllProducts();
        IEnumerable<ProductResponse> result = products.Select(product => MapToDto(product)).ToList();

        return Ok(result);
    }

    [HttpGet("user")]
    [Authorize]
    public ActionResult<IEnumerable<ProductResponse>> GetUserProductsList()
    {
        int? userId = GetUserIdFromToken();
        if (!userId.HasValue || !ValidateUserId(userId.Value))
        {
            return BadRequest();
        }

        IEnumerable<Product> products = _productService.GetAllProducts();
        IEnumerable<ProductResponse> result = products.Select(product => MapToDto(product)).ToList();

        return Ok(result);
    }

    [HttpPost("user")]
    [Authorize]
    public ActionResult<ProductResponse> CreateProduct(ProductRequest productRequest, char categoryLetter)
    {
        int? userId = GetUserIdFromToken();

        if (!userId.HasValue || !ValidateUserId(userId.Value))
        {
            return BadRequest();
        }
        Category category = _categoryService.GetCategoryByLetter(categoryLetter);
        User? user = _userService.GetUserById(userId.Value);

        if (user is null)
        {
            return BadRequest();
        }

        Product product = new(productRequest.Name, productRequest.Description, productRequest.Price, productRequest.Brand, category, user);


        Product createdProduct = _productService.CreateProduct(product);
        if (createdProduct == null)
        {
            return BadRequest("Can't create the product");
        }

        return Ok(MapToDto(createdProduct));
    }

    [HttpPut("user/{productId}")]
    [Authorize]
    public async Task<ActionResult<ProductResponse>> UpdateProduct(ProductRequest productRequest, char categoryLetter, int productId)
    {
        int? userId = GetUserIdFromToken();

        if (!userId.HasValue || !ValidateUserId(userId.Value))
        {
            return BadRequest();
        }
        Category category = _categoryService.GetCategoryByLetter(categoryLetter);
        User? user = _userService.GetUserById(userId.Value);
        Product? product = _productService.GetProductById(productId);

        if (user is null || category is null || product is null)
        {
            return BadRequest();
        }

        product.Name = productRequest.Name;
        product.Description = productRequest.Description;
        product.Price = productRequest.Price;
        product.Brand = productRequest.Brand;
        product.Category = category;

        bool result = await _productService.UpdateProduct(product);
        if (!result)
        {
            return BadRequest();
        }

        return Ok();
    }

    [HttpDelete("user/{productId}")]
    [Authorize]
    public async Task<ActionResult<ProductResponse>> DeleteProduct(int productId)
    {
        int? userId = GetUserIdFromToken();

        if (!userId.HasValue || !ValidateUserId(userId.Value))
        {
            return BadRequest();
        }
        User? user = _userService.GetUserById(userId.Value);
        Product? product = _productService.GetProductById(productId);

        if (user is null || product is null || product.User.Id != user.Id)
        {
            return BadRequest();
        }

        bool result = await _productService.DeleteProduct(product);
        if (!result)
        {
            return BadRequest();
        }

        return Ok();
    }
}
