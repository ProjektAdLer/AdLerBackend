using AdLerBackend.API.Properties;
using AdLerBackend.Infrastructure.Repositories.BaseContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace AdLerBackend.Infrastructure.Repositories;

public sealed class ProductionContext : BaseAdLerBackendDbContext
{
    private readonly BackendConfig _backendConfig;

    public ProductionContext(DbContextOptions options, IOptions<BackendConfig> confguration) : base(options)
    {
        _backendConfig = confguration.Value;
        Database.EnsureCreated();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        var password = _backendConfig.DBPassword;
        var username = _backendConfig.DBUser;
        var name = _backendConfig.DBName;
        var dbhost = _backendConfig.DBHost;
        var dbPort = _backendConfig.DBPort;

        // connection string for mariaDB
        var connectionString = $"server={dbhost};port={dbPort};user={username};database={name};password={password}";

        options.UseMySql(connectionString,
            new MariaDbServerVersion(new Version(10, 9, 2)));
    }
}