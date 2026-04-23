using System.Collections.Generic;

namespace DesktopShop.Application.DTOs.Dashboard;

public class RevenueByRangeDto
{
    public string FromDate { get; set; } = string.Empty;
    public string ToDate { get; set; } = string.Empty;
    public int TotalDays { get; set; }

    public decimal TotalRevenue { get; set; }
    public int TotalOrderCount { get; set; }
    public decimal AverageDailyRevenue { get; set; }

    // Daily breakdown for chart
    public List<string> DailyLabels { get; set; } = new();
    public List<decimal> DailyValues { get; set; } = new();
    public List<int> DailyOrderCounts { get; set; } = new();
}
