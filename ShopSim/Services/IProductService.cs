using ShopSim.DTOs;

namespace ShopSim.Services;

public interface IProductService
{
    Task <IEnumerable<ProductReadDto>> GetAllProducts();
    Task <ProductReadDto> GetProductById(int id);
    Task <ProductReadDto> CreateProduct(ProductCreateDto productCreateDto);
    Task <bool> UpdateProduct(int id, ProductUpdateDto productUpdateDto);
    Task <bool> DeleteProduct(int id);
}