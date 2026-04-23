namespace DesktopShop.Application.DTOs.Product;

public class UpdateProductDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int CategoryId { get; set; }
    public string? CPU { get; set; }
    public string? RAM { get; set; }
    public string? GPU { get; set; }
    public string? Storage { get; set; }
    public decimal Price { get; set; }
    public int StockQuantity { get; set; }
    public int MinStockLevel { get; set; }
    public string? ImageUrl { get; set; }
    public string? Description { get; set; }
    public bool IsActive { get; set; }
}
