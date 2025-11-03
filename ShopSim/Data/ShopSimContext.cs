using Microsoft.EntityFrameworkCore;
using ShopSim.Models;

namespace ShopSim.Data;

public class ShopSimContext(DbContextOptions<ShopSimContext> options) : DbContext(options)
{
    public DbSet<Product> Products { get; set; }
}