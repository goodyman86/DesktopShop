namespace DesktopShop.Application.DTOs.Dashboard;

public class RevenueByDateDto
{
    public string SelectedDate { get; set; } = string.Empty;

    // Revenue of the selected day
    public decimal DayRevenue { get; set; }
    public int DayOrderCount { get; set; }

    // Revenue of the month containing the selected day
    public decimal MonthRevenue { get; set; }
    public int MonthOrderCount { get; set; }
    public string MonthLabel { get; set; } = string.Empty;

    // Revenue of the year containing the selected day
    public decimal YearRevenue { get; set; }
    public int YearOrderCount { get; set; }
    public string YearLabel { get; set; } = string.Empty;
}
