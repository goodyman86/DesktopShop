namespace DesktopShop.Domain.Entities;

public class OrderDetail : BaseEntity
{
    public int OrderId { get; set; }
    public int ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public decimal UnitPrice { get; set; }
    public int Quantity { get; set; }
    public decimal SubTotal { get; set; }

    public Order Order { get; set; } = null!;
    public Product Product { get; set; } = null!;
}
