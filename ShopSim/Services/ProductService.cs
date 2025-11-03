using AutoMapper;
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
        var product = _mapper.Map<Product>(productCreateDto); 
        _context.Products.Add(product);
        await _context.SaveChangesAsync();
        return _mapper.Map<ProductReadDto>(product);
    }

    public async Task<bool> UpdateProduct(int id, ProductUpdateDto productUpdateDto)
    {
        var item = await _context.Products.FindAsync(id);
        if (item == null)
        {
            return false;
        }
        _mapper.Map(productUpdateDto, item);
        _context.Products.Update(item);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteProduct(int id)
    {
        var item = await _context.Products.FindAsync(id);
        if (item == null)
        {
            return false;
        }
        _context.Products.Remove(item);
        await _context.SaveChangesAsync();
        return true;
    }
}