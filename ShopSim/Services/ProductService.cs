using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ShopSim.Data;
using ShopSim.DTOs;

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
    
    public async Task<IEnumerable<ProductReadDto>> GetAllProducts()
    {
        var items = await _context.Products.ToListAsync();
        return items.Select(item => _mapper.Map<ProductReadDto>(item));
    }

    public async Task<ProductReadDto> GetProductById(int id)
    {
        var item = await _context.Products.FindAsync(id);
        return item == null ? throw new KeyNotFoundException("Item não encontrado") : _mapper.Map<ProductReadDto>(item);
    }

    public async Task<ProductReadDto> CreateProduct(ProductCreateDto productCreateDto)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> UpdateProduct(int id, ProductUpdateDto productUpdateDto)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> DeleteProduct(int id)
    {
        throw new NotImplementedException();
    }
}