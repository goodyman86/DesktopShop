using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DesktopShop.Domain.Entities;

namespace DesktopShop.Domain.Interfaces;

public interface IOrderRepository : IRepository<Order>
{
    Task<Order?> GetOrderWithDetailsAsync(int id);
    Task<IEnumerable<Order>> GetOrdersByDateRangeAsync(DateTime from, DateTime to);
    Task<string> GenerateOrderCodeAsync();
}
