using System.Diagnostics.CodeAnalysis;
using AdLerBackend.Application.Configuration;
using AdLerBackend.Infrastructure.Repositories.BaseContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace AdLerBackend.Infrastructure.Repositories;

[ExcludeFromCodeCoverage]
public sealed class ProductionContext : BaseAdLerBackendDbContext
{
    private readonly BackendConfig _backendConfig;

    public ProductionContext(DbContextOptions options, IOptions<BackendConfig> confguration) : base(options)
    {
        _backendConfig = confguration.Value;
        Database.Migrate();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        var password = _backendConfig.DbPassword;
        var username = _backendConfig.DbUser;
        var name = _backendConfig.DbName;
        var dbhost = _backendConfig.DbHost;
        var dbPort = _backendConfig.DbPort;

        // connection string for mariaDB
        var connectionString = $"server={dbhost};port={dbPort};user={username};database={name};password={password}";

        options.UseMySql(connectionString,
            new MariaDbServerVersion(new Version(10, 9, 2)));
    }
}