using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage;
using DesktopShop.Domain.Interfaces;
using DesktopShop.Infrastructure.Data;

namespace DesktopShop.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    private IDbContextTransaction? _transaction;

    public ICategoryRepository Categories { get; }
    public IProductRepository Products { get; }
    public IOrderRepository Orders { get; }

    public UnitOfWork(ApplicationDbContext context,
        ICategoryRepository categoryRepository,
        IProductRepository productRepository,
        IOrderRepository orderRepository)
    {
        _context = context;
        Categories = categoryRepository;
        Products = productRepository;
        Orders = orderRepository;
    }

    public async Task<int> SaveChangesAsync()
        => await _context.SaveChangesAsync();

    public async Task BeginTransactionAsync()
    {
        _transaction = await _context.Database.BeginTransactionAsync();
    }

    public async Task CommitTransactionAsync()
    {
        if (_transaction != null)
        {
            await _transaction.CommitAsync();
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public async Task RollbackTransactionAsync()
    {
        if (_transaction != null)
        {
            await _transaction.RollbackAsync();
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public void Dispose()
    {
        _transaction?.Dispose();
        _context.Dispose();
    }
}
