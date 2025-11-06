using Microsoft.AspNetCore.Mvc;
using ShopSim.DTOs;
using ShopSim.Services;

namespace ShopSim.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly ICategoryService _categoryService;

    public CategoriesController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllCategories([FromQuery] PaginationFilter filter)
    {
        try
        {
            var result = await _categoryService.GetAllCategoriesAsync(filter);
            return Ok(ApiResponse<PagedResult<CategoryReadDto>>.SuccessResponse(result));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<object>.ErrorResponse("An error occurred while retrieving categories"));
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetCategoryById(int id)
    {
        try
        {
            var category = await _categoryService.GetCategoryByIdAsync(id);
            return Ok(ApiResponse<CategoryReadDto>.SuccessResponse(category));
        }
        catch (KeyNotFoundException)
        {
            return NotFound(ApiResponse<object>.ErrorResponse("Category not found"));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<object>.ErrorResponse("An error occurred while retrieving the category"));
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateCategory([FromBody] CategoryCreateDto categoryCreateDto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ApiResponse<object>.ErrorResponse("Invalid input",
                    ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList()));
            }

            var createdCategory = await _categoryService.CreateCategoryAsync(categoryCreateDto);
            
            return CreatedAtAction(nameof(GetCategoryById), 
                new { id = createdCategory.Id }, 
                ApiResponse<CategoryReadDto>.SuccessResponse(createdCategory, "Category created successfully"));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<object>.ErrorResponse("An error occurred while creating the category"));
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCategory(int id, [FromBody] CategoryUpdateDto categoryUpdateDto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ApiResponse<object>.ErrorResponse("Invalid input",
                    ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList()));
            }

            var updated = await _categoryService.UpdateCategoryAsync(id, categoryUpdateDto);

            if (!updated)
            {
                return NotFound(ApiResponse<object>.ErrorResponse("Category not found"));
            }

            return Ok(ApiResponse<object>.SuccessResponse(null, "Category updated successfully"));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<object>.ErrorResponse("An error occurred while updating the category"));
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCategory(int id)
    {
        try
        {
            var deleted = await _categoryService.DeleteCategoryAsync(id);

            if (!deleted)
            {
                return NotFound(ApiResponse<object>.ErrorResponse("Category not found"));
            }

            return Ok(ApiResponse<object>.SuccessResponse(null, "Category deleted successfully"));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ApiResponse<object>.ErrorResponse(ex.Message));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<object>.ErrorResponse("An error occurred while deleting the category"));
        }
    }
}
