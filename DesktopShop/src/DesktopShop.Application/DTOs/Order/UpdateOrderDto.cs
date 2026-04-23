using DesktopShop.Domain.Enums;
using System.Collections.Generic;

namespace DesktopShop.Application.DTOs.Order;

public class UpdateOrderDto
{
    public string CustomerName { get; set; } = string.Empty;
    public string? CustomerPhone { get; set; }
    public string? CustomerEmail { get; set; }
    public string? ShippingAddress { get; set; }
    public string? Notes { get; set; }
    public PaymentMethod PaymentMethod { get; set; }
    public List<UpdateOrderDetailDto> Items { get; set; } = new();
}

public class UpdateOrderDetailDto
{
    public int ProductId { get; set; }
    public int Quantity { get; set; }
}
