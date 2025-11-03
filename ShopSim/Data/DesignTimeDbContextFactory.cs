using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace ShopSim.Data;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ShopSimContext>
{
    public ShopSimContext CreateDbContext(string[] args)
    {
        var builder = new DbContextOptionsBuilder<ShopSimContext>();
        
        // Para design-time, use localhost já que o comando roda fora do Docker
        builder.UseMySql(
            "Server=localhost;Port=3307;Database=ShopSim;Uid=appuser;Pwd=AppPassword123!;",
            new MySqlServerVersion(new Version(8, 0, 33))
        );

        return new ShopSimContext(builder.Options);
    }
}
