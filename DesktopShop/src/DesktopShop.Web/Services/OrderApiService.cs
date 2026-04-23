using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DesktopShop.Application.DTOs.Order;

namespace DesktopShop.Web.Services;

public class OrderApiService
{
    private readonly IApiService _api;

    public OrderApiService(IApiService api)
    {
        _api = api;
    }

    public Task<IEnumerable<OrderDto>?> GetAllAsync()
        => _api.GetAsync<IEnumerable<OrderDto>>("api/orders");

    public Task<OrderDto?> GetByIdAsync(int id)
        => _api.GetAsync<OrderDto>($"api/orders/{id}");

    public Task<OrderDto?> CreateAsync(CreateOrderDto dto)
        => _api.PostAsync<OrderDto>("api/orders", dto);

    public Task<bool> UpdateStatusAsync(int id, int status)
        => _api.PatchAsync($"api/orders/{id}/status", status);

    public Task<IEnumerable<OrderDto>?> GetByDateRangeAsync(DateTime from, DateTime to)
        => _api.GetAsync<IEnumerable<OrderDto>>(
            $"api/orders/by-date?from={from:yyyy-MM-dd}&to={to:yyyy-MM-dd}");
}
