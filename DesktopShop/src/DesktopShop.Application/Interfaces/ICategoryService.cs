using System.Collections.Generic;
using System.Threading.Tasks;
using DesktopShop.Application.DTOs.Category;

namespace DesktopShop.Application.Interfaces;

public interface ICategoryService
{
    Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync();
    Task<IEnumerable<CategoryDto>> GetActiveCategoriesAsync();
    Task<CategoryDto?> GetCategoryByIdAsync(int id);
    Task<CategoryDto> CreateCategoryAsync(CreateCategoryDto dto);
    Task<CategoryDto?> UpdateCategoryAsync(UpdateCategoryDto dto);
    Task<bool> DeleteCategoryAsync(int id);
}
