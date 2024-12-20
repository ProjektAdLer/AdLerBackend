using AdLerBackend.Application.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Options;

namespace AdLerBackend.Infrastructure.Repositories;

public class ProductionContextFactory : IDesignTimeDbContextFactory<ProductionContext>
{
    public ProductionContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ProductionContext>();

        var config = new BackendConfig
        {
            DbHost = "localhost",
            DbPort = "3306",
            DbUser = "adler_backend",
            DbPassword = "b",
            DbName = "adler_backend"
        };

        return new ProductionContext(
            optionsBuilder.Options,
            Options.Create(config)
        );
    }
}