using System.Collections.Generic;

namespace DesktopShop.Application.DTOs.Dashboard;

public class DashboardSummaryDto
{
    public int TotalProducts { get; set; }
    public int TotalOrders { get; set; }
    public decimal TotalRevenue { get; set; }
    public int LowStockCount { get; set; }
    public int OutOfStockCount { get; set; }
    public List<TopSellingProductDto> TopSellingProducts { get; set; } = new();
}

public class TopSellingProductDto
{
    public string ProductName { get; set; } = string.Empty;
    public int TotalSold { get; set; }
    public decimal TotalRevenue { get; set; }
}
