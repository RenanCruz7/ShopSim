using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ShopSim.Data;
using ShopSim.DTOs;
using ShopSim.Models;

namespace ShopSim.Services;

public class CategoryService : ICategoryService
{
    private readonly ShopSimContext _context;
    private readonly IMapper _mapper;

    public CategoryService(ShopSimContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PagedResult<CategoryReadDto>> GetAllCategoriesAsync(PaginationFilter filter)
    {
        var query = _context.Categories.AsQueryable();

        // Apply search filter
        if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
        {
            query = query.Where(c => c.Name.Contains(filter.SearchTerm) || 
                                   c.Description.Contains(filter.SearchTerm));
        }

        // Apply sorting
        query = filter.SortDirection.ToLower() == "desc" 
            ? query.OrderByDescending(GetSortExpression(filter.SortBy))
            : query.OrderBy(GetSortExpression(filter.SortBy));

        var totalCount = await query.CountAsync();

        var categories = await query
            .Skip((filter.Page - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .Include(c => c.Products)
            .ToListAsync();

        var categoryDtos = categories.Select(c => new CategoryReadDto
        {
            Id = c.Id,
            Name = c.Name,
            Description = c.Description,
            IsActive = c.IsActive,
            CreatedAt = c.CreatedAt,
            UpdatedAt = c.UpdatedAt,
            ProductCount = c.Products.Count
        }).ToList();

        return new PagedResult<CategoryReadDto>
        {
            Data = categoryDtos,
            TotalCount = totalCount,
            Page = filter.Page,
            PageSize = filter.PageSize
        };
    }

    public async Task<CategoryReadDto> GetCategoryByIdAsync(int id)
    {
        var category = await _context.Categories
            .Include(c => c.Products)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (category == null)
        {
            throw new KeyNotFoundException("Category not found");
        }

        return new CategoryReadDto
        {
            Id = category.Id,
            Name = category.Name,
            Description = category.Description,
            IsActive = category.IsActive,
            CreatedAt = category.CreatedAt,
            UpdatedAt = category.UpdatedAt,
            ProductCount = category.Products.Count
        };
    }

    public async Task<CategoryReadDto> CreateCategoryAsync(CategoryCreateDto categoryCreateDto)
    {
        var category = _mapper.Map<Category>(categoryCreateDto);
        
        _context.Categories.Add(category);
        await _context.SaveChangesAsync();

        return _mapper.Map<CategoryReadDto>(category);
    }

    public async Task<bool> UpdateCategoryAsync(int id, CategoryUpdateDto categoryUpdateDto)
    {
        var category = await _context.Categories.FindAsync(id);
        if (category == null)
        {
            return false;
        }

        _mapper.Map(categoryUpdateDto, category);
        category.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteCategoryAsync(int id)
    {
        var category = await _context.Categories
            .Include(c => c.Products)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (category == null)
        {
            return false;
        }

        if (category.Products.Any())
        {
            throw new InvalidOperationException("Cannot delete category with associated products");
        }

        _context.Categories.Remove(category);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> CategoryExistsAsync(int id)
    {
        return await _context.Categories.AnyAsync(c => c.Id == id);
    }

    private static System.Linq.Expressions.Expression<Func<Category, object>> GetSortExpression(string sortBy)
    {
        return sortBy.ToLower() switch
        {
            "name" => c => c.Name,
            "createdat" => c => c.CreatedAt,
            "updatedat" => c => c.UpdatedAt ?? DateTime.MinValue,
            _ => c => c.Id
        };
    }
}
