using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DesktopShop.Domain.Entities;
using DesktopShop.Domain.Interfaces;
using DesktopShop.Infrastructure.Data;

namespace DesktopShop.Infrastructure.Repositories;

public class OrderRepository : Repository<Order>, IOrderRepository
{
    public OrderRepository(ApplicationDbContext context) : base(context) { }

    public async Task<Order?> GetOrderWithDetailsAsync(int id)
        => await _dbSet.Include(o => o.OrderDetails)
                       .ThenInclude(od => od.Product)
                       .FirstOrDefaultAsync(o => o.Id == id);

    public async Task<IEnumerable<Order>> GetOrdersByDateRangeAsync(DateTime from, DateTime to)
        => await _dbSet.Include(o => o.OrderDetails)
                       .Where(o => o.CreatedAt >= from && o.CreatedAt <= to)
                       .OrderByDescending(o => o.CreatedAt)
                       .ToListAsync();

    public async Task<string> GenerateOrderCodeAsync()
    {
        var today = DateTime.UtcNow.ToString("yyyyMMdd");
        var count = await _dbSet.CountAsync(o => o.OrderCode.StartsWith($"ORD-{today}"));
        return $"ORD-{today}-{(count + 1):D4}";
    }
}
