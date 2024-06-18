using eEcomerce.BackEnd.Controllers;
using eEcomerce.BackEnd.Entities.Order;
using eEcomerce.BackEnd.Entities.OrderProduct;
using eEcomerce.BackEnd.Entities.Product;
using eEcomerce.BackEnd.Entities.User;
using eEcomerce.BackEnd.Models.Order;
using eEcomerce.BackEnd.Models.OrderProduct;
using eEcomerce.BackEnd.Services.Order;
using eEcomerce.BackEnd.Services.OrderProduct;
using eEcomerce.BackEnd.Services.Product;
using eEcomerce.BackEnd.Services.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Moq;

using UnitTestingeEcomerce.Tests.Utils;

namespace UnitTestingeEcomerce.Tests.ControllersTests;

public class OrderControllerTests
{
    private readonly Mock<IProductService> _mockProductService;
    private readonly Mock<IOrderService> _mockOrderService;
    private readonly Mock<IOrderProductService> _mockOrderProductService;
    private readonly Mock<IHttpContextAccessor> _mockHttpContextAccessor;
    private readonly Mock<IUserService> _mockUserService;
    private readonly OrderController _controller;
    private readonly UserTesterBuilder _userBuilder;

    public OrderControllerTests()
    {
        _mockProductService = new Mock<IProductService>();
        _mockOrderService = new Mock<IOrderService>();
        _mockOrderProductService = new Mock<IOrderProductService>();
        _mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
        _mockUserService = new Mock<IUserService>();
        _controller = new OrderController(
            _mockProductService.Object,
            _mockHttpContextAccessor.Object,
            _mockUserService.Object,
            _mockOrderService.Object,
            _mockOrderProductService.Object
        );
        _userBuilder = new UserTesterBuilder(_mockHttpContextAccessor);
    }

    [Fact]
    public void GetUserOrdersList_InvalidUserId_ReturnsBadRequest()
    {
        // Arrange
        _userBuilder.WithId(Guid.Empty).SetupUserInHttpContext();

        // Act
        ActionResult<IEnumerable<OrderResponse>> result = _controller.GetUserOrdersList();

        // Assert
        BadRequestObjectResult badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
        Assert.Equal("Invalid user ID.", badRequestResult.Value);
    }

    [Fact]
    public void GetUserOrdersList_ValidUserId_ReturnsOrderList()
    {
        // Arrange
        Guid userId = Guid.NewGuid();
        _userBuilder.WithId(userId).SetupUserInHttpContext();

        List<Order> orders = new()
        {
            new Order { Id = Guid.NewGuid(), DatePurchase = DateTime.UtcNow, TotalPrice = 100 },
            new Order { Id = Guid.NewGuid(), DatePurchase = DateTime.UtcNow, TotalPrice = 200 }
        };

        _mockUserService.Setup(x => x.GetUserById(userId)).Returns(new User());
        _mockOrderService.Setup(x => x.GetUserOrders(userId)).Returns(orders);

        // Act
        ActionResult<IEnumerable<OrderResponse>> result = _controller.GetUserOrdersList();

        // Assert
        OkObjectResult okResult = Assert.IsType<OkObjectResult>(result.Result);
        List<OrderResponse> returnValue = Assert.IsType<List<OrderResponse>>(okResult.Value);

        Assert.Equal(2, returnValue.Count);
    }

    [Fact]
    public void GetUserOrderProductsList_InvalidUserId_ReturnsBadRequest()
    {
        // Arrange
        _userBuilder.WithId(Guid.Empty).SetupUserInHttpContext();
        Guid orderId = Guid.NewGuid();

        // Act
        ActionResult<IEnumerable<OrderProductResponse>> result = _controller.GetUserOrderProductsList(orderId);

        // Assert
        BadRequestObjectResult badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
        Assert.Equal("Invalid user ID.", badRequestResult.Value);
    }

    [Fact]
    public void GetUserOrderProductsList_ValidUserId_ReturnsOrderProductList()
    {
        // Arrange
        Guid userId = Guid.NewGuid();
        _userBuilder.WithId(userId).SetupUserInHttpContext();
        Guid orderId = Guid.NewGuid();

        List<OrderProduct> orderProducts = new()
        {
            new OrderProduct { ProductId = Guid.NewGuid(), Quantity = 2, ProductValue = 100 },
            new OrderProduct { ProductId = Guid.NewGuid(), Quantity = 3, ProductValue = 150 }
        };
        _mockUserService.Setup(x => x.GetUserById(userId)).Returns(new User());
        _mockOrderProductService.Setup(x => x.GetOrderProducts_ByOrderId(orderId)).Returns(orderProducts);
        _mockProductService.Setup(x => x.GetProductById(It.IsAny<Guid>())).Returns(new Product());

        // Act
        ActionResult<IEnumerable<OrderProductResponse>> result = _controller.GetUserOrderProductsList(orderId);

        // Assert
        OkObjectResult okResult = Assert.IsType<OkObjectResult>(result.Result);
        List<OrderProductResponse> returnValue = Assert.IsType<List<OrderProductResponse>>(okResult.Value);

        Assert.Equal(2, returnValue.Count);
    }

    [Fact]
    public void CreateUserOrder_InvalidUserId_ReturnsBadRequest()
    {
        // Arrange
        _userBuilder.WithId(Guid.Empty).SetupUserInHttpContext();
        OrderRequest orderRequest = new() { ListOrderProducts = new List<OrderProductRequest>() };

        // Act
        ActionResult<OrderResponse> result = _controller.CreateUserOrder(orderRequest);

        // Assert
        BadRequestObjectResult badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
        Assert.Equal("Invalid user ID.", badRequestResult.Value);
    }

    [Fact]
    public void CreateUserOrder_ProductNotFound_ReturnsBadRequest()
    {
        // Arrange
        Guid userId = Guid.NewGuid();
        _userBuilder.WithId(userId).SetupUserInHttpContext();

        OrderRequest orderRequest = new()
        {
            ListOrderProducts = new List<OrderProductRequest>
            {
                new() { ProductId = Guid.NewGuid(), Quantity = 1 }
            }
        };
        _mockUserService.Setup(x => x.GetUserById(userId)).Returns(new User());
        _mockProductService.Setup(x => x.GetProductById(It.IsAny<Guid>())).Returns((Product) null);

        // Act
        ActionResult<OrderResponse> result = _controller.CreateUserOrder(orderRequest);

        // Assert
        BadRequestObjectResult badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
        Assert.Equal("Product not found.", badRequestResult.Value);
    }

    [Fact]
    public void CreateUserOrder_Success_ReturnsOrderResponse()
    {
        // Arrange
        Guid userId = Guid.NewGuid();
        _userBuilder.WithId(userId).SetupUserInHttpContext();

        List<OrderProductRequest> orderProductsRequest = new()
        {
            new OrderProductRequest { ProductId = Guid.NewGuid(), Quantity = 2 },
            new OrderProductRequest { ProductId = Guid.NewGuid(), Quantity = 3 }
        };

        OrderRequest orderRequest = new() { ListOrderProducts = orderProductsRequest };

        User user = new() { Id = userId };
        Product product = new() { Id = Guid.NewGuid(), Price = 50 };
        Order order = new() { Id = Guid.NewGuid(), DatePurchase = DateTime.UtcNow, TotalPrice = 250 };
        Order createdOrder = new() { Id = Guid.NewGuid(), DatePurchase = DateTime.UtcNow, TotalPrice = 250 };

        _mockUserService.Setup(x => x.GetUserById(userId)).Returns(user);
        _mockProductService.Setup(x => x.GetProductById(It.IsAny<Guid>())).Returns(product);
        _mockOrderService.Setup(x => x.CreateOrder(It.IsAny<Order>())).Returns(createdOrder);

        _mockOrderProductService.Setup(x => x.CreateOrderProduct(It.IsAny<OrderProduct>())).Returns(new OrderProduct());

        // Act
        ActionResult<OrderResponse> result = _controller.CreateUserOrder(orderRequest);

        // Assert
        OkObjectResult okResult = Assert.IsType<OkObjectResult>(result.Result);
        OrderResponse returnValue = Assert.IsType<OrderResponse>(okResult.Value);

        Assert.Equal(createdOrder.Id, returnValue.Id);
    }
}