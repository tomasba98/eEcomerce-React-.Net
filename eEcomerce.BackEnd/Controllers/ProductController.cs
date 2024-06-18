using eEcomerce.BackEnd.Entities.Category;
using eEcomerce.BackEnd.Entities.Product;
using eEcomerce.BackEnd.Entities.User;
using eEcomerce.BackEnd.Models.Product;
using eEcomerce.BackEnd.Services.Category;
using eEcomerce.BackEnd.Services.Product;
using eEcomerce.BackEnd.Services.User;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eEcomerce.BackEnd.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : BaseController
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;

        public ProductController(IProductService productService, IHttpContextAccessor httpContextAccessor, IUserService userService, ICategoryService categoryService)
            : base(httpContextAccessor, userService)
        {
            _productService = productService;
            _categoryService = categoryService;
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
            IEnumerable<ProductResponse> result = products.Select(MapToDto).ToList();
            return Ok(result);
        }

        [HttpGet("users")]
        [Authorize]
        public ActionResult<IEnumerable<ProductResponse>> GetUserProductsList()
        {
            Guid? userId = GetUserIdFromToken();

            if (!ValidateUserId(userId))
            {
                return BadRequest("Invalid user ID.");
            }

            IEnumerable<Product> products = _productService.GetUserProducts(userId.Value);
            IEnumerable<ProductResponse> result = products.Select(MapToDto).ToList();

            return Ok(result);
        }

        [HttpPost("users")]
        [Authorize]
        public ActionResult<ProductResponse> CreateProduct(ProductRequest productRequest, char categoryLetter)
        {
            Guid? userId = GetUserIdFromToken();

            if (!ValidateUserId(userId))
            {
                return BadRequest("Invalid user ID.");
            }

            User user = _userService.GetUserById(userId.Value);
            Category category = _categoryService.GetCategoryByLetter(categoryLetter);

            Product product = new(productRequest.Name, productRequest.Description, productRequest.Price, productRequest.Brand, category, user);
            Product createdProduct = _productService.CreateProduct(product);

            if (createdProduct == null)
            {
                return BadRequest("Can't create the product.");
            }

            return Ok(MapToDto(createdProduct));
        }

        [HttpPut("users/{productId}")]
        [Authorize]
        public async Task<ActionResult<ProductResponse>> UpdateProduct(ProductRequest productRequest, char categoryLetter, Guid productId)
        {
            Guid? userId = GetUserIdFromToken();

            if (!ValidateUserId(userId))
            {
                return BadRequest("Invalid user ID.");
            }

            Category category = _categoryService.GetCategoryByLetter(categoryLetter);
            User user = _userService.GetUserById(userId.Value);
            Product product = _productService.GetProductById(productId);

            if (user == null || category == null || product == null)
            {
                return BadRequest("Invalid data.");
            }

            product.Name = productRequest.Name;
            product.Description = productRequest.Description;
            product.Price = productRequest.Price;
            product.Brand = productRequest.Brand;
            product.Category = category;

            bool result = await _productService.UpdateProduct(product);

            if (!result)
            {
                return BadRequest("Failed to update the product.");
            }

            return Ok(MapToDto(product));
        }

        [HttpDelete("users/{productId}")]
        [Authorize]
        public async Task<ActionResult<ProductResponse>> DeleteProduct(Guid productId)
        {
            Guid? userId = GetUserIdFromToken();

            if (!ValidateUserId(userId))
            {
                return BadRequest("Invalid user ID.");
            }

            User user = _userService.GetUserById(userId.Value);
            Product product = _productService.GetProductById(productId);

            if (user == null || product == null || product.User.Id != user.Id)
            {
                return BadRequest("Invalid data.");
            }

            bool result = await _productService.DeleteProduct(product);

            if (!result)
            {
                return BadRequest("Failed to delete the product.");
            }

            return Ok();
        }
    }
}
