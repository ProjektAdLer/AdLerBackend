using System.Diagnostics.CodeAnalysis;
using AdLerBackend.Infrastructure.Repositories.BaseContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace AdLerBackend.Infrastructure.Repositories;

[ExcludeFromCodeCoverage]
public sealed class DevelopmentContext : BaseAdLerBackendDbContext
{
    private readonly IConfiguration _configuration;

    public DevelopmentContext(DbContextOptions options, IConfiguration configuration) : base(options)
    {
        _configuration = configuration;
        Database.Migrate();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        // get password from environment variable
        options.UseSqlite(_configuration.GetConnectionString("DevelopomentConnection"));
    }
}