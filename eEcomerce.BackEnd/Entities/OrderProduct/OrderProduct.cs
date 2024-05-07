﻿namespace eEcomerce.BackEnd.Entities.OrderProduct;

using eEcomerce.BackEnd.Entities.Order;
using eEcomerce.BackEnd.Entities.Product;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("OrderProducts")]
public class OrderProduct : EntityBase
{
    public OrderProduct()
    {
    }
    public OrderProduct(int quantity, Order order, Product product)
    {
        Quantity = quantity;
        ProductValue = product.Price;
        Order = order;
        OrderId = order.Id;
        Product = product;
        ProductId = product.Id;
    }

    public int Quantity { get; set; }

    public decimal ProductValue { get; set; }

    [Required]
    [Column("OrderId")]
    public required Order Order { get; set; }
    public int OrderId { get; set; }

    [Required]
    [Column("ProductId")]
    public required Product Product { get; set; }
    public int ProductId { get; set; }

}
