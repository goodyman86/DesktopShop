using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using DesktopShop.Application.DTOs.Product;

namespace DesktopShop.Web.Services;

public class ProductApiService
{
    private readonly IApiService _api;

    public ProductApiService(IApiService api)
    {
        _api = api;
    }

    public Task<IEnumerable<ProductDto>?> GetAllAsync()
        => _api.GetAsync<IEnumerable<ProductDto>>("api/products");

    public Task<ProductDto?> GetByIdAsync(int id)
        => _api.GetAsync<ProductDto>($"api/products/{id}");

    public Task<IEnumerable<ProductDto>?> GetByCategoryAsync(int categoryId)
        => _api.GetAsync<IEnumerable<ProductDto>>($"api/products/category/{categoryId}");

    public Task<IEnumerable<ProductDto>?> GetLowStockAsync()
        => _api.GetAsync<IEnumerable<ProductDto>>("api/products/low-stock");

    public Task<IEnumerable<ProductDto>?> SearchAsync(string keyword)
        => _api.GetAsync<IEnumerable<ProductDto>>($"api/products/search?keyword={Uri.EscapeDataString(keyword)}");

    public Task<ProductDto?> CreateAsync(CreateProductDto dto)
        => _api.PostAsync<ProductDto>("api/products", dto);

    public Task<ProductDto?> UpdateAsync(UpdateProductDto dto)
        => _api.PutAsync<ProductDto>($"api/products/{dto.Id}", dto);

    public Task<bool> DeleteAsync(int id)
        => _api.DeleteAsync($"api/products/{id}");

    public async Task<string> UploadImageAsync(string fileName, Stream fileStream)
    {
        using (var content = new MultipartFormDataContent())
        {
            var fileContent = new StreamContent(fileStream);
            content.Add(fileContent, "file", fileName);

            var result = await _api.PostFormAsync<ImageUploadResult>("api/products/upload-image", content);
            return result?.ImageUrl;
        }
    }

    private class ImageUploadResult
    {
        public string ImageUrl { get; set; } = string.Empty;
    }
}
