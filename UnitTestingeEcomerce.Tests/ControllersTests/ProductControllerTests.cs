using eEcomerce.BackEnd.Controllers;
using eEcomerce.BackEnd.Entities.Category;
using eEcomerce.BackEnd.Entities.Product;
using eEcomerce.BackEnd.Entities.User;
using eEcomerce.BackEnd.Models.Product;
using eEcomerce.BackEnd.Services.Category;
using eEcomerce.BackEnd.Services.Product;
using eEcomerce.BackEnd.Services.Users;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Moq;

using UnitTestingeEcomerce.Tests.Utils;

namespace UnitTestingeEcomerce.Tests.ControllersTests
{
    public class ProductControllerTests
    {
        private readonly Mock<IProductService> _mockProductService;
        private readonly Mock<ICategoryService> _mockCategoryService;
        private readonly Mock<IHttpContextAccessor> _mockHttpContextAccessor;
        private readonly Mock<IUserService> _mockUserService;
        private readonly ProductController _controller;
        private readonly UserTesterBuilder _userBuilder;

        public ProductControllerTests()
        {
            _mockProductService = new Mock<IProductService>();
            _mockCategoryService = new Mock<ICategoryService>();
            _mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            _mockUserService = new Mock<IUserService>();
            _controller = new ProductController(
                _mockProductService.Object,
                _mockHttpContextAccessor.Object,
                _mockUserService.Object,
                _mockCategoryService.Object
            );
            _userBuilder = new UserTesterBuilder(_mockHttpContextAccessor);
        }

        [Fact]
        public void GetProductsList_ReturnsProductList()
        {
            // Arrange
            List<Product> products = new()
            {
                new Product { Id = Guid.NewGuid(), Name = "Product 1", Brand = "Brand 1", Price = 100 },
                new Product { Id = Guid.NewGuid(), Name = "Product 2", Brand = "Brand 2", Price = 200 }
            };

            _mockProductService.Setup(x => x.GetAllProducts()).Returns(products);

            // Act
            ActionResult<IEnumerable<ProductResponse>> result = _controller.GetProductsList();

            // Assert
            OkObjectResult okResult = Assert.IsType<OkObjectResult>(result.Result);
            List<ProductResponse> returnValue = Assert.IsType<List<ProductResponse>>(okResult.Value);

            Assert.Equal(2, returnValue.Count);
        }

        [Fact]
        public void GetUserProductsList_InvalidUserId_ReturnsBadRequest()
        {
            // Arrange
            _userBuilder.WithId(Guid.Empty).SetupUserInHttpContext();

            // Act
            ActionResult<IEnumerable<ProductResponse>> result = _controller.GetUserProductsList();

            // Assert
            BadRequestObjectResult badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal("Invalid user ID.", badRequestResult.Value);
        }

        [Fact]
        public void GetUserProductsList_ValidUserId_ReturnsProductList()
        {
            // Arrange
            Guid userId = Guid.NewGuid();
            _userBuilder.WithId(userId).SetupUserInHttpContext();

            List<Product> products = new()
            {
                new Product { Id = Guid.NewGuid(), Name = "Product 1", Brand = "Brand 1", Price = 100 },
                new Product { Id = Guid.NewGuid(), Name = "Product 2", Brand = "Brand 2", Price = 200 }
            };
            _mockUserService.Setup(x => x.GetUserById(userId)).Returns(new User());
            _mockProductService.Setup(x => x.GetUserProducts(userId)).Returns(products);

            // Act
            ActionResult<IEnumerable<ProductResponse>> result = _controller.GetUserProductsList();

            // Assert
            OkObjectResult okResult = Assert.IsType<OkObjectResult>(result.Result);
            List<ProductResponse> returnValue = Assert.IsType<List<ProductResponse>>(okResult.Value);

            Assert.Equal(2, returnValue.Count);
        }

        [Fact]
        public void CreateProduct_InvalidUserId_ReturnsBadRequest()
        {
            // Arrange
            _userBuilder.WithId(Guid.Empty).SetupUserInHttpContext();
            ProductRequest productRequest = new();
            char categoryLetter = 'A';

            // Act
            ActionResult<ProductResponse> result = _controller.CreateProduct(productRequest, categoryLetter);

            // Assert
            BadRequestObjectResult badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal("Invalid user ID.", badRequestResult.Value);
        }

        [Fact]
        public void CreateProduct_CannotCreateProduct_ReturnsBadRequest()
        {
            // Arrange
            Guid userId = Guid.NewGuid();
            _userBuilder.WithId(userId).SetupUserInHttpContext();
            ProductRequest productRequest = new() { Name = "Product", Description = "Description", Price = 100, Brand = "Brand" };
            char categoryLetter = 'A';

            User user = new() { Id = userId };
            Category category = new() { Letter = categoryLetter };

            _mockUserService.Setup(x => x.GetUserById(userId)).Returns(user);
            _mockCategoryService.Setup(x => x.GetCategoryByLetter(categoryLetter)).Returns(category);
            _mockProductService.Setup(x => x.CreateProduct(It.IsAny<Product>())).Returns((Product) null);

            // Act
            ActionResult<ProductResponse> result = _controller.CreateProduct(productRequest, categoryLetter);

            // Assert
            BadRequestObjectResult badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal("Can't create the product.", badRequestResult.Value);
        }

        [Fact]
        public void CreateProduct_Success_ReturnsProductResponse()
        {
            // Arrange
            Guid userId = Guid.NewGuid();
            _userBuilder.WithId(userId).SetupUserInHttpContext();
            ProductRequest productRequest = new() { Name = "Product", Description = "Description", Price = 100, Brand = "Brand" };
            char categoryLetter = 'A';

            User user = new() { Id = userId };
            Category category = new() { Letter = categoryLetter };
            Product product = new() { Id = Guid.NewGuid(), Name = "Product", Description = "Description", Price = 100, Brand = "Brand", Category = category, User = user };
            Product createdProduct = product;

            _mockUserService.Setup(x => x.GetUserById(userId)).Returns(user);
            _mockCategoryService.Setup(x => x.GetCategoryByLetter(categoryLetter)).Returns(category);
            _mockProductService.Setup(x => x.CreateProduct(It.IsAny<Product>())).Returns(createdProduct);

            // Act
            ActionResult<ProductResponse> result = _controller.CreateProduct(productRequest, categoryLetter);

            // Assert
            OkObjectResult okResult = Assert.IsType<OkObjectResult>(result.Result);
            ProductResponse returnValue = Assert.IsType<ProductResponse>(okResult.Value);

            Assert.Equal(createdProduct.Id, returnValue.Id);
        }

        [Fact]
        public async Task UpdateProduct_InvalidUserId_ReturnsBadRequest()
        {
            // Arrange
            _userBuilder.WithId(Guid.Empty).SetupUserInHttpContext();
            ProductRequest productRequest = new();
            char categoryLetter = 'A';
            Guid productId = Guid.NewGuid();

            // Act
            ActionResult<ProductResponse> result = await _controller.UpdateProduct(productRequest, categoryLetter, productId);

            // Assert
            BadRequestObjectResult badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal("Invalid user ID.", badRequestResult.Value);
        }

        [Fact]
        public async Task UpdateProduct_InvalidData_ReturnsBadRequest()
        {
            // Arrange
            Guid userId = Guid.NewGuid();
            _userBuilder.WithId(userId).SetupUserInHttpContext();
            ProductRequest productRequest = new();
            char categoryLetter = 'A';
            Guid productId = Guid.NewGuid();

            _mockCategoryService.Setup(x => x.GetCategoryByLetter(categoryLetter)).Returns((Category) null);
            _mockUserService.Setup(x => x.GetUserById(userId)).Returns(new User());
            _mockProductService.Setup(x => x.GetProductById(productId)).Returns((Product) null);

            // Act
            ActionResult<ProductResponse> result = await _controller.UpdateProduct(productRequest, categoryLetter, productId);

            // Assert
            BadRequestObjectResult badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal("Invalid data.", badRequestResult.Value);
        }

        [Fact]
        public async Task UpdateProduct_FailedToUpdate_ReturnsBadRequest()
        {
            // Arrange
            Guid userId = Guid.NewGuid();
            _userBuilder.WithId(userId).SetupUserInHttpContext();
            ProductRequest productRequest = new() { Name = "Updated Product", Description = "Updated Description", Price = 150, Brand = "Updated Brand" };
            char categoryLetter = 'A';
            Guid productId = Guid.NewGuid();

            Category category = new() { Letter = categoryLetter };
            User user = new() { Id = userId };
            Product product = new() { Id = productId, Name = "Product", Description = "Description", Price = 100, Brand = "Brand", Category = category, User = user };

            _mockCategoryService.Setup(x => x.GetCategoryByLetter(categoryLetter)).Returns(category);
            _mockUserService.Setup(x => x.GetUserById(userId)).Returns(user);
            _mockProductService.Setup(x => x.GetProductById(productId)).Returns(product);
            _mockProductService.Setup(x => x.UpdateProduct(It.IsAny<Product>())).ReturnsAsync(false);

            // Act
            ActionResult<ProductResponse> result = await _controller.UpdateProduct(productRequest, categoryLetter, productId);

            // Assert
            BadRequestObjectResult badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal("Failed to update the product.", badRequestResult.Value);
        }

        [Fact]
        public async Task UpdateProduct_Success_ReturnsProductResponse()
        {
            // Arrange
            Guid userId = Guid.NewGuid();
            _userBuilder.WithId(userId).SetupUserInHttpContext();
            ProductRequest productRequest = new() { Name = "Updated Product", Description = "Updated Description", Price = 150, Brand = "Updated Brand" };
            char categoryLetter = 'A';
            Guid productId = Guid.NewGuid();

            Category category = new() { Letter = categoryLetter };
            User user = new() { Id = userId };
            Product product = new() { Id = productId, Name = "Product", Description = "Description", Price = 100, Brand = "Brand", Category = category, User = user };

            _mockCategoryService.Setup(x => x.GetCategoryByLetter(categoryLetter)).Returns(category);
            _mockUserService.Setup(x => x.GetUserById(userId)).Returns(user);
            _mockProductService.Setup(x => x.GetProductById(productId)).Returns(product);
            _mockProductService.Setup(x => x.UpdateProduct(It.IsAny<Product>())).ReturnsAsync(true);

            // Act
            ActionResult<ProductResponse> result = await _controller.UpdateProduct(productRequest, categoryLetter, productId);

            // Assert
            OkObjectResult okResult = Assert.IsType<OkObjectResult>(result.Result);
            ProductResponse returnValue = Assert.IsType<ProductResponse>(okResult.Value);

            Assert.Equal(product.Id, returnValue.Id);
        }

        [Fact]
        public async Task DeleteProduct_InvalidUserId_ReturnsBadRequest()
        {
            // Arrange
            _userBuilder.WithId(Guid.Empty).SetupUserInHttpContext();
            Guid productId = Guid.NewGuid();

            // Act
            ActionResult<ProductResponse> result = await _controller.DeleteProduct(productId);

            // Assert
            BadRequestObjectResult badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal("Invalid user ID.", badRequestResult.Value);
        }

        [Fact]
        public async Task DeleteProduct_InvalidData_ReturnsBadRequest()
        {
            // Arrange
            Guid userId = Guid.NewGuid();
            _userBuilder.WithId(userId).SetupUserInHttpContext();
            Guid productId = Guid.NewGuid();

            _mockUserService.Setup(x => x.GetUserById(userId)).Returns(new User());
            _mockProductService.Setup(x => x.GetProductById(productId)).Returns((Product) null);

            // Act
            ActionResult<ProductResponse> result = await _controller.DeleteProduct(productId);

            // Assert
            BadRequestObjectResult badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal("Invalid data.", badRequestResult.Value);
        }

        [Fact]
        public async Task DeleteProduct_FailedToDelete_ReturnsBadRequest()
        {
            // Arrange
            Guid userId = Guid.NewGuid();
            _userBuilder.WithId(userId).SetupUserInHttpContext();
            Guid productId = Guid.NewGuid();

            User user = new() { Id = userId };
            Product product = new() { Id = productId, User = user };

            _mockUserService.Setup(x => x.GetUserById(userId)).Returns(user);
            _mockProductService.Setup(x => x.GetProductById(productId)).Returns(product);
            _mockProductService.Setup(x => x.DeleteProduct(It.IsAny<Product>())).ReturnsAsync(false);

            // Act
            ActionResult<ProductResponse> result = await _controller.DeleteProduct(productId);

            // Assert
            BadRequestObjectResult badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal("Failed to delete the product.", badRequestResult.Value);
        }

        [Fact]
        public async Task DeleteProduct_Success_ReturnsOk()
        {
            // Arrange
            Guid userId = Guid.NewGuid();
            _userBuilder.WithId(userId).SetupUserInHttpContext();
            Guid productId = Guid.NewGuid();

            User user = new() { Id = userId };
            Product product = new() { Id = productId, User = user };

            _mockUserService.Setup(x => x.GetUserById(userId)).Returns(user);
            _mockProductService.Setup(x => x.GetProductById(productId)).Returns(product);
            _mockProductService.Setup(x => x.DeleteProduct(It.IsAny<Product>())).ReturnsAsync(true);

            // Act
            ActionResult<ProductResponse> result = await _controller.DeleteProduct(productId);

            // Assert
            OkResult okResult = Assert.IsType<OkResult>(result.Result);
        }
    }
}
