using AdLerBackend.Infrastructure.Repositories.BaseContext;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace AdLerBackend.Infrastructure.UnitTests.Repositories;

public static class ContextCreator
{
    public static BaseAdLerBackendDbContext GetNewDbContextInstance()
    {
        var connection = new SqliteConnection("DataSource=:memory:");
        connection.Open();
        var options = new DbContextOptionsBuilder<BaseAdLerBackendDbContext>()
            .UseSqlite(connection)
            .Options;
        var context = new BaseAdLerBackendDbContext(options);
        context.Database.EnsureCreated();
        return context;
    }
}