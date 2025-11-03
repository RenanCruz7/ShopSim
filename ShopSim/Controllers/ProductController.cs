using Microsoft.AspNetCore.Mvc;
using ShopSim.DTOs;
using ShopSim.Services;

namespace ShopSim.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductController : ControllerBase
{

    private readonly IProductService _productService;
    public ProductController(IProductService productService)
    {
        _productService = productService;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAllProducts()
    {
        var products = await _productService.GetAllProducts();
        return new OkObjectResult(products);
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetProductById(int id)
    {
        try
        {
            var product = await _productService.GetProductById(id);
            return new OkObjectResult(product);
        }
        catch (KeyNotFoundException)
        {
            return new NotFoundObjectResult(new { message = "Product not found" });
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateProduct([FromBody] ProductCreateDto productCreateDto)
    {
        if (!ModelState.IsValid)
        {
            return new BadRequestObjectResult(ModelState);
        }
        
        var createdProduct = await _productService.CreateProduct(productCreateDto);
        return new CreatedAtActionResult("GetProductById", "Product", new { id = createdProduct.Id }, createdProduct);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProduct(int id, [FromBody] ProductUpdateDto productUpdateDto)
    {
        if (!ModelState.IsValid)
        {
            return new BadRequestObjectResult(ModelState);
        }
        
        var updated = await _productService.UpdateProduct(id, productUpdateDto);
        
        if (!updated)
        {
            return new NotFoundObjectResult(new { message = "Product not found" });
        }
        
        return new OkObjectResult(new { message = "Product updated successfully" });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        var deleted = await _productService.DeleteProduct(id);
        
        if (!deleted)
        {
            return new NotFoundObjectResult(new { message = "Product not found" });
        }
        
        return new OkObjectResult(new { message = "Product deleted successfully" });
    }
}