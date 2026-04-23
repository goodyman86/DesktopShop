using System.Collections.Generic;
using System.Threading.Tasks;
using DesktopShop.Application.DTOs.Category;

namespace DesktopShop.Web.Services;

public class CategoryApiService
{
    private readonly IApiService _api;

    public CategoryApiService(IApiService api)
    {
        _api = api;
    }

    public Task<IEnumerable<CategoryDto>?> GetAllAsync()
        => _api.GetAsync<IEnumerable<CategoryDto>>("api/categories");

    public Task<IEnumerable<CategoryDto>?> GetActiveAsync()
        => _api.GetAsync<IEnumerable<CategoryDto>>("api/categories/active");

    public Task<CategoryDto?> GetByIdAsync(int id)
        => _api.GetAsync<CategoryDto>($"api/categories/{id}");

    public Task<CategoryDto?> CreateAsync(CreateCategoryDto dto)
        => _api.PostAsync<CategoryDto>("api/categories", dto);

    public Task<CategoryDto?> UpdateAsync(UpdateCategoryDto dto)
        => _api.PutAsync<CategoryDto>($"api/categories/{dto.Id}", dto);

    public Task<bool> DeleteAsync(int id)
        => _api.DeleteAsync($"api/categories/{id}");
}
