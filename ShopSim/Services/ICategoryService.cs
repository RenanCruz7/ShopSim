using ShopSim.DTOs;

namespace ShopSim.Services;

public interface ICategoryService
{
    Task<PagedResult<CategoryReadDto>> GetAllCategoriesAsync(PaginationFilter filter);
    Task<CategoryReadDto> GetCategoryByIdAsync(int id);
    Task<CategoryReadDto> CreateCategoryAsync(CategoryCreateDto categoryCreateDto);
    Task<bool> UpdateCategoryAsync(int id, CategoryUpdateDto categoryUpdateDto);
    Task<bool> DeleteCategoryAsync(int id);
    Task<bool> CategoryExistsAsync(int id);
}
