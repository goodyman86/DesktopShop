using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using DesktopShop.Application.DTOs.Product;
using DesktopShop.Application.Interfaces;
using DesktopShop.Domain.Entities;
using DesktopShop.Domain.Interfaces;

namespace DesktopShop.Application.Services;

public class ProductService : IProductService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ProductService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ProductDto>> GetAllProductsAsync()
    {
        var products = await _unitOfWork.Products.GetAllWithCategoryAsync();
        return _mapper.Map<IEnumerable<ProductDto>>(products);
    }

    public async Task<ProductDto?> GetProductByIdAsync(int id)
    {
        var product = await _unitOfWork.Products.GetProductWithCategoryAsync(id);
        return product == null ? null : _mapper.Map<ProductDto>(product);
    }

    public async Task<IEnumerable<ProductDto>> GetProductsByCategoryAsync(int categoryId)
    {
        var products = await _unitOfWork.Products.GetProductsByCategoryAsync(categoryId);
        return _mapper.Map<IEnumerable<ProductDto>>(products);
    }

    public async Task<IEnumerable<ProductDto>> GetLowStockProductsAsync()
    {
        var products = await _unitOfWork.Products.GetLowStockProductsAsync();
        return _mapper.Map<IEnumerable<ProductDto>>(products);
    }

    public async Task<IEnumerable<ProductDto>> SearchProductsAsync(string keyword)
    {
        var products = await _unitOfWork.Products.SearchProductsAsync(keyword);
        return _mapper.Map<IEnumerable<ProductDto>>(products);
    }

    public async Task<ProductDto> CreateProductAsync(CreateProductDto dto)
    {
        var product = _mapper.Map<Product>(dto);
        await _unitOfWork.Products.AddAsync(product);
        await _unitOfWork.SaveChangesAsync();

        var created = await _unitOfWork.Products.GetProductWithCategoryAsync(product.Id);
        return _mapper.Map<ProductDto>(created!);
    }

    public async Task<ProductDto?> UpdateProductAsync(UpdateProductDto dto)
    {
        var product = await _unitOfWork.Products.GetByIdAsync(dto.Id);
        if (product == null) return null;

        _mapper.Map(dto, product);
        _unitOfWork.Products.Update(product);
        await _unitOfWork.SaveChangesAsync();

        var updated = await _unitOfWork.Products.GetProductWithCategoryAsync(product.Id);
        return _mapper.Map<ProductDto>(updated!);
    }

    public async Task<bool> DeleteProductAsync(int id)
    {
        var product = await _unitOfWork.Products.GetByIdAsync(id);
        if (product == null) return false;

        _unitOfWork.Products.Delete(product);
        await _unitOfWork.SaveChangesAsync();
        return true;
    }
}
