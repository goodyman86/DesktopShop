using System;
using System.Threading.Tasks;

namespace DesktopShop.Domain.Interfaces;

public interface IUnitOfWork : IDisposable
{
    ICategoryRepository Categories { get; }
    IProductRepository Products { get; }
    IOrderRepository Orders { get; }
    Task<int> SaveChangesAsync();
    Task BeginTransactionAsync();
    Task CommitTransactionAsync();
    Task RollbackTransactionAsync();
}
