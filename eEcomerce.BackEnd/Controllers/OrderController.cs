using eEcomerce.BackEnd.Entities.Order;
using eEcomerce.BackEnd.Entities.OrderProduct;
using eEcomerce.BackEnd.Entities.Product;
using eEcomerce.BackEnd.Entities.User;
using eEcomerce.BackEnd.Models.Order;
using eEcomerce.BackEnd.Models.OrderProduct;
using eEcomerce.BackEnd.Models.Product;
using eEcomerce.BackEnd.Services.Order;
using eEcomerce.BackEnd.Services.OrderProduct;
using eEcomerce.BackEnd.Services.Product;
using eEcomerce.BackEnd.Services.Users;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eEcomerce.BackEnd.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : BaseController
    {
        private readonly IProductService _productService;
        private readonly IOrderService _orderService;
        private readonly IOrderProductService _orderProductService;

        public OrderController(
            IProductService productService,
            IHttpContextAccessor httpContextAccessor,
            IUserService userService,
            IOrderService orderService,
            IOrderProductService orderProductService)
            : base(httpContextAccessor, userService)
        {
            _productService = productService;
            _orderService = orderService;
            _orderProductService = orderProductService;
        }

        private Product? GetProductById(Guid productId)
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
                TotalPrice = order.TotalPrice
            };
        }

        private OrderProductResponse MapToDtoOrder_ProductResponse(OrderProduct orderProducts)
        {

            return new OrderProductResponse
            {
                Quantity = orderProducts.Quantity,
                ProductValue = orderProducts.ProductValue,
                Proudct = MapToDto_Product(GetProductById(orderProducts.ProductId))

            };
        }

        [HttpGet("user")]
        [Authorize]
        public ActionResult<IEnumerable<OrderResponse>> GetUserOrdersList()
        {
            Guid? userId = GetUserIdFromToken();

            if (!ValidateUserId(userId))
            {
                return BadRequest("Invalid user ID.");
            }

            IEnumerable<Order> orders = _orderService.GetUserOrders(userId.Value);
            IEnumerable<OrderResponse> result = orders.Select(MapToDto_OrderResponse).ToList();

            return Ok(result);
        }

        [HttpGet("user/{orderId}")]
        [Authorize]
        public ActionResult<IEnumerable<OrderProductResponse>> GetUserOrderProductsList(Guid orderId)
        {
            Guid? userId = GetUserIdFromToken();

            if (!ValidateUserId(userId))
            {
                return BadRequest("Invalid user ID.");
            }

            IEnumerable<OrderProduct> orderProducts = _orderProductService.GetOrderProducts_ByOrderId(orderId);
            IEnumerable<OrderProductResponse> result = orderProducts.Select(MapToDtoOrder_ProductResponse).ToList();

            return Ok(result);
        }

        [HttpPost("user")]
        [Authorize]
        public ActionResult<OrderResponse> CreateUserOrder(OrderRequest orderRequest)
        {
            Guid? userId = GetUserIdFromToken();

            if (!ValidateUserId(userId))
            {
                return BadRequest("Invalid user ID.");
            }

            User? user = _userService.GetUserById(userId.Value);

            decimal totalOrderPrice = 0;
            foreach (OrderProductRequest productOrder in orderRequest.ListOrderProducts)
            {
                Product? product = GetProductById(productOrder.ProductId);
                if (product == null)
                {
                    return BadRequest("Product not found.");
                }

                totalOrderPrice += product.Price * productOrder.Quantity;
            }

            int totalProducts = orderRequest.ListOrderProducts.Sum(p => p.Quantity);

            Order order = new(totalOrderPrice, user, totalProducts);
            Order createdOrder = _orderService.CreateOrder(order);
            if (createdOrder == null)
            {
                return BadRequest("Can't create the order.");
            }

            foreach (OrderProductRequest productOrder in orderRequest.ListOrderProducts)
            {
                Product? product = GetProductById(productOrder.ProductId);
                if (product == null)
                {
                    return BadRequest("Product not found.");
                }

                OrderProduct orderProduct = new(productOrder.Quantity, createdOrder, product);
                OrderProduct createdOrderProduct = _orderProductService.CreateOrderProduct(orderProduct);
                if (createdOrderProduct == null)
                {
                    return BadRequest("Can't create the order product.");
                }
            }

            return Ok(MapToDto_OrderResponse(createdOrder));
        }
    }
}
