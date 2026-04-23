using System.Collections.Generic;

namespace DesktopShop.Application.DTOs.Dashboard;

public class SalesChartDto
{
    public List<string> Labels { get; set; } = new();
    public List<decimal> Values { get; set; } = new();
}
