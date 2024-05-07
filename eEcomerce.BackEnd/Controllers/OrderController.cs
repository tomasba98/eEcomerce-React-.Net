﻿using eEcomerce.BackEnd.Entities.Order;
using eEcomerce.BackEnd.Entities.OrderProduct;
using eEcomerce.BackEnd.Entities.Product;
using eEcomerce.BackEnd.Models.Order;
using eEcomerce.BackEnd.Models.OrderProduct;
using eEcomerce.BackEnd.Models.Product;
using eEcomerce.BackEnd.Services.Order;
using eEcomerce.BackEnd.Services.OrderProduct;
using eEcomerce.BackEnd.Services.Product;
using eEcomerce.BackEnd.Services.Users;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using System.Security.Claims;

namespace eEcomerce.BackEnd.Controllers;


[ApiController]
[Route("api/[controller]")]
public class OrderController : ControllerBase
{
    private readonly IProductService _productService;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IUserService _userService;
    private readonly IOrderService _orderService;
    private readonly IOrderProductService _oderProdcutService;

    public OrderController(IProductService productService, IHttpContextAccessor httpContextAccessor, IUserService userService, IOrderService orderService, IOrderProductService orderProductService)
    {
        _productService = productService;
        _httpContextAccessor = httpContextAccessor;
        _userService = userService;
        _orderService = orderService;
        _oderProdcutService = orderProductService;
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
        if (_userService.GetUserById(userId) is null)
        {
            return false;
        }
        return true;
    }

    private Product GetProductById(int productId)
    {
        return _productService.GetProductById(productId);
    }
    private static ProductResponse MapToDto_Product(Product product)
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
    private static OrderResponse MapToDto_OrderResponse(Order order)
    {
        return new OrderResponse
        {
            Id = order.Id,
            DatePurchase = order.DatePurchase,
            TotalPrice = order.TotalPrice,

        };
    }
    private OrderProductResponse MapToDtoOrder_ProductResponse(OrderProduct orderProducts)
    {

        return new OrderProductResponse
        {
            Quantity = orderProducts.Quantity,
            ProductValue = orderProducts.ProductValue,
            proudct = MapToDto_Product(GetProductById(orderProducts.ProductId))

        };
    }

    [HttpGet("user/orders")]
    [Authorize]
    public ActionResult<IEnumerable<OrderResponse>> GetUserOrdersList()
    {
        int? userId = GetUserIdFromToken();
        if (!userId.HasValue || !ValidateUserId(userId.Value))
        {
            return BadRequest();
        }

        IEnumerable<Order> orders = _orderService.GetUserOrders(userId);
        IEnumerable<OrderResponse> result = orders.Select(ord => MapToDto_OrderResponse(ord)).ToList();

        return Ok(result);
    }

    [HttpGet("user/orders/{orderId}")]
    [Authorize]
    public ActionResult<IEnumerable<OrderProductResponse>> GetUserOrderProductsList(int orderId)
    {
        int? userId = GetUserIdFromToken();
        if (!userId.HasValue || !ValidateUserId(userId.Value))
        {
            return BadRequest();
        }

        IEnumerable<OrderProduct> orderProducts = _oderProdcutService.GetOrderProducts_ByOrderId(orderId);
        IEnumerable<OrderProductResponse> result = orderProducts.Select(ordProd => MapToDtoOrder_ProductResponse(ordProd)).ToList();

        return Ok(result);
    }

    //[HttpPost("user/create-order")]
    //[Authorize]
    //public ActionResult<OrderRequest> CreateUserOrder(OrderRequest orderRequest)
    //{
    //    int? userId = GetUserIdFromToken();

    //    if (!userId.HasValue || !ValidateUserId(userId.Value))
    //    {
    //        return BadRequest();
    //    }
    //    int Quantity = orderRequest.OrderProducts.Count();
    //    User? user = _userService.GetUserById(userId.Value);
    //    Order order = new(orderProduct, user);
    //    OrderProduct orderProduct = new(orderRequest)

    //    Order createdOrder = _orderService.CreateOrder(order);
    //    if (createdOrder == null)
    //    {
    //        return BadRequest("Can't create the task");
    //    }

    //    return Ok(MapToDto(createdOrder));
    //}

}
