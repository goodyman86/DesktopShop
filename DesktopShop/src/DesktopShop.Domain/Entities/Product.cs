using System.Collections.Generic;

namespace DesktopShop.Domain.Entities;

public class Product : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public int CategoryId { get; set; }
    public string? CPU { get; set; }
    public string? RAM { get; set; }
    public string? GPU { get; set; }
    public string? Storage { get; set; }
    public decimal Price { get; set; }
    public int StockQuantity { get; set; }
    public int MinStockLevel { get; set; } = 5;
    public string? ImageUrl { get; set; }
    public string? Description { get; set; }
    public bool IsActive { get; set; } = true;

    public Category Category { get; set; } = null!;
    public ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
}
