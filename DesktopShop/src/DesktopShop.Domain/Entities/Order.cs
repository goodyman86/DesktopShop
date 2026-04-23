using System.Collections.Generic;
using DesktopShop.Domain.Enums;

namespace DesktopShop.Domain.Entities;

public class Order : BaseEntity
{
    public string OrderCode { get; set; } = string.Empty;
    public string CustomerName { get; set; } = string.Empty;
    public string? CustomerPhone { get; set; }
    public string? CustomerEmail { get; set; }
    public string? ShippingAddress { get; set; }
    public decimal TotalAmount { get; set; }
    public OrderStatus Status { get; set; } = OrderStatus.Pending;
    public PaymentMethod PaymentMethod { get; set; } = PaymentMethod.CashOnDelivery;
    public string? Notes { get; set; }
    public string? CreatedByUserId { get; set; }

    public ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
}
