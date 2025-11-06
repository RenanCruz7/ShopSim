﻿using ShopSim.DTOs;

namespace ShopSim.Services;

public interface IProductService
{
    Task<PagedResult<ProductReadDto>> GetAllProductsAsync(ProductFilter filter);
    Task<ProductReadDto> GetProductByIdAsync(int id);
    Task<ProductReadDto> CreateProductAsync(ProductCreateDto productCreateDto);
    Task<bool> UpdateProductAsync(int id, ProductUpdateDto productUpdateDto);
    Task<bool> DeleteProductAsync(int id);
    Task<List<ProductReadDto>> GetProductsByCategoryAsync(int categoryId);
    Task<bool> ProductExistsAsync(int id);
}