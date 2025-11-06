﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ShopSim.Data;
using ShopSim.DTOs;
using ShopSim.Models;

namespace ShopSim.Services;

public class ProductService : IProductService
{
    private readonly ShopSimContext _context;
    private readonly IMapper _mapper;

    public ProductService(ShopSimContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PagedResult<ProductReadDto>> GetAllProductsAsync(ProductFilter filter)
    {
        var query = _context.Products
            .Include(p => p.Category)
            .Where(p => p.IsActive)
            .AsQueryable();

        // Apply filters
        if (filter.CategoryId.HasValue)
        {
            query = query.Where(p => p.CategoryId == filter.CategoryId.Value);
        }

        if (filter.MinPrice.HasValue)
        {
            query = query.Where(p => p.Price >= filter.MinPrice.Value);
        }

        if (filter.MaxPrice.HasValue)
        {
            query = query.Where(p => p.Price <= filter.MaxPrice.Value);
        }

        if (filter.InStock.HasValue && filter.InStock.Value)
        {
            query = query.Where(p => p.StockQuantity > 0);
        }

        // Apply search filter
        if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
        {
            query = query.Where(p => p.Name.Contains(filter.SearchTerm) ||
                                   p.Description.Contains(filter.SearchTerm) ||
                                   p.SKU.Contains(filter.SearchTerm));
        }

        // Apply sorting
        query = filter.SortDirection.ToLower() == "desc"
            ? query.OrderByDescending(GetSortExpression(filter.SortBy))
            : query.OrderBy(GetSortExpression(filter.SortBy));

        var totalCount = await query.CountAsync();

        var products = await query
            .Skip((filter.Page - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .ToListAsync();

        var productDtos = products.Select(p => new ProductReadDto
        {
            Id = p.Id,
            Name = p.Name,
            Description = p.Description,
            Price = p.Price,
            StockQuantity = p.StockQuantity,
            CategoryId = p.CategoryId,
            CategoryName = p.Category.Name,
            ImageUrl = p.ImageUrl,
            SKU = p.SKU,
            IsActive = p.IsActive,
            CreatedAt = p.CreatedAt,
            UpdatedAt = p.UpdatedAt
        }).ToList();

        return new PagedResult<ProductReadDto>
        {
            Data = productDtos,
            TotalCount = totalCount,
            Page = filter.Page,
            PageSize = filter.PageSize
        };
    }

    public async Task<ProductReadDto> GetProductByIdAsync(int id)
    {
        var product = await _context.Products
            .Include(p => p.Category)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (product == null)
        {
            throw new KeyNotFoundException("Product not found");
        }

        return new ProductReadDto
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            StockQuantity = product.StockQuantity,
            CategoryId = product.CategoryId,
            CategoryName = product.Category.Name,
            ImageUrl = product.ImageUrl,
            SKU = product.SKU,
            IsActive = product.IsActive,
            CreatedAt = product.CreatedAt,
            UpdatedAt = product.UpdatedAt
        };
    }

    public async Task<ProductReadDto> CreateProductAsync(ProductCreateDto productCreateDto)
    {
        // Validate category exists
        var categoryExists = await _context.Categories.AnyAsync(c => c.Id == productCreateDto.CategoryId);
        if (!categoryExists)
        {
            throw new KeyNotFoundException("Category not found");
        }

        var product = _mapper.Map<Product>(productCreateDto);

        _context.Products.Add(product);
        await _context.SaveChangesAsync();

        return await GetProductByIdAsync(product.Id);
    }

    public async Task<bool> UpdateProductAsync(int id, ProductUpdateDto productUpdateDto)
    {
        var product = await _context.Products.FindAsync(id);

        if (product == null)
        {
            return false;
        }

        // Validate category exists
        var categoryExists = await _context.Categories.AnyAsync(c => c.Id == productUpdateDto.CategoryId);
        if (!categoryExists)
        {
            throw new KeyNotFoundException("Category not found");
        }

        _mapper.Map(productUpdateDto, product);
        product.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteProductAsync(int id)
    {
        var product = await _context.Products
            .Include(p => p.OrderItems)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (product == null)
        {
            return false;
        }

        if (product.OrderItems.Any())
        {
            // Soft delete - just mark as inactive
            product.IsActive = false;
            product.UpdatedAt = DateTime.UtcNow;
        }
        else
        {
            // Hard delete if no orders reference this product
            _context.Products.Remove(product);
        }

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<List<ProductReadDto>> GetProductsByCategoryAsync(int categoryId)
    {
        var products = await _context.Products
            .Include(p => p.Category)
            .Where(p => p.CategoryId == categoryId && p.IsActive)
            .ToListAsync();

        return products.Select(p => new ProductReadDto
        {
            Id = p.Id,
            Name = p.Name,
            Description = p.Description,
            Price = p.Price,
            StockQuantity = p.StockQuantity,
            CategoryId = p.CategoryId,
            CategoryName = p.Category.Name,
            ImageUrl = p.ImageUrl,
            SKU = p.SKU,
            IsActive = p.IsActive,
            CreatedAt = p.CreatedAt,
            UpdatedAt = p.UpdatedAt
        }).ToList();
    }

    public async Task<bool> ProductExistsAsync(int id)
    {
        return await _context.Products.AnyAsync(p => p.Id == id);
    }

    private static System.Linq.Expressions.Expression<Func<Product, object>> GetSortExpression(string sortBy)
    {
        return sortBy.ToLower() switch
        {
            "name" => p => p.Name,
            "price" => p => p.Price,
            "stock" => p => p.StockQuantity,
            "createdat" => p => p.CreatedAt,
            "updatedat" => p => p.UpdatedAt ?? DateTime.MinValue,
            "category" => p => p.Category.Name,
            _ => p => p.Id
        };
    }
}

