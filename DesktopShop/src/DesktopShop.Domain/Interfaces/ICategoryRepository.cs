using System.Collections.Generic;
using System.Threading.Tasks;
using DesktopShop.Domain.Entities;

namespace DesktopShop.Domain.Interfaces;

public interface ICategoryRepository : IRepository<Category>
{
    Task<Category?> GetByNameAsync(string name);
    Task<IEnumerable<Category>> GetActiveCategoriesAsync();
}
