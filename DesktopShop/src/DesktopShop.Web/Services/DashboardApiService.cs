using System.Threading.Tasks;
using DesktopShop.Application.DTOs.Dashboard;

namespace DesktopShop.Web.Services;

public class DashboardApiService
{
    private readonly IApiService _api;

    public DashboardApiService(IApiService api)
    {
        _api = api;
    }

    public Task<DashboardSummaryDto?> GetSummaryAsync()
        => _api.GetAsync<DashboardSummaryDto>("api/dashboard/summary");

    public Task<SalesChartDto?> GetSalesChartAsync(int months = 6)
        => _api.GetAsync<SalesChartDto>($"api/dashboard/sales-chart?months={months}");
}
