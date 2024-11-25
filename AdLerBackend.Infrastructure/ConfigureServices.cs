using System.Diagnostics.CodeAnalysis;
using System.IO.Abstractions;
using AdLerBackend.Application.Common.Interfaces;
using AdLerBackend.Infrastructure.LmsBackup;
using AdLerBackend.Infrastructure.Moodle;
using AdLerBackend.Infrastructure.Repositories;
using AdLerBackend.Infrastructure.Repositories.BaseContext;
using AdLerBackend.Infrastructure.Repositories.Common;
using AdLerBackend.Infrastructure.Repositories.Worlds;
using AdLerBackend.Infrastructure.Services;
using AdLerBackend.Infrastructure.Storage;
using Microsoft.Extensions.DependencyInjection;

namespace AdLerBackend.Infrastructure;

[ExcludeFromCodeCoverage]
public static class ConfigureServices
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, bool isDevelopment)
    {
        // Add Moodle to DI
        services.AddSingleton<ILMS, MoodleWebApi>();
        services.AddSingleton<ILmsBackupProcessor, LmsBackupProcessor>();
        services.AddScoped<IFileAccess, StorageService>();
        services.AddSingleton<ISerialization, SerializationService>();
        services.AddScoped<IWorldRepository, WorldRepository>();
        services.AddScoped(typeof(IGenericRepository<,>), typeof(GenericRepository<,>));
        services.AddTransient<IFileSystem, FileSystem>();
        services.AddSingleton(new HttpClient());


        if (isDevelopment)
            services.AddDbContext<BaseAdLerBackendDbContext, DevelopmentContext>();
        else
            services.AddDbContext<BaseAdLerBackendDbContext, ProductionContext>();


        return services;
    }
}