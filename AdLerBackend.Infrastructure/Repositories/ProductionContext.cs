using System.Diagnostics.CodeAnalysis;
using AdLerBackend.Infrastructure.Repositories.BaseContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace AdLerBackend.Infrastructure.Repositories;

[ExcludeFromCodeCoverage]
public sealed class ProductionContext : BaseAdLerBackendDbContext
{
    private readonly IConfiguration _configuration;

    public ProductionContext(DbContextOptions options, IConfiguration configuration) : base(options)
    {
        _configuration = configuration;
        Database.EnsureCreated();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        var password = _configuration["ASPNETCORE_DBPASSWORD"];
        var username = _configuration["ASPNETCORE_DBUSER"];
        var name = _configuration["ASPNETCORE_DBNAME"];
        var dbhost = _configuration["ASPNETCORE_DBHOST"];
        var dbPort = _configuration["ASPNETCORE_DBPORT"];

        // connection string for mariaDB
        var connectionString = $"server={dbhost};port={dbPort};user={username};database={name};password={password}";

        options.UseMySql(connectionString,
            new MariaDbServerVersion(new Version(10, 9, 2)));
    }
}