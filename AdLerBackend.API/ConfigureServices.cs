using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using AdLerBackend.API.Filters;
using AdLerBackend.API.Middleware;
using AdLerBackend.Application.Configuration;
using AdLerBackend.Infrastructure.Repositories.BaseContext;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using UnzipMiddleware;

namespace AdLerBackend.API;

/// <summary>
///     This class is used to configure all services needed for the API
/// </summary>
[ExcludeFromCodeCoverage]
public static class ConfigureServices
{
    /// <summary>
    ///     Add all services needed for the API
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddApiServices(this IServiceCollection services)
    {
        services
            .AddControllers(options => { options.Filters.Add<ApiExceptionFilterAttribute>(); });

        services.AddHttpContextAccessor();

        services.AddEndpointsApiExplorer();
        services.AddHealthChecks()
            .AddDbContextCheck<BaseAdLerBackendDbContext>(
                "database",
                HealthStatus.Unhealthy,
                new[] {"db", "database"});
        services.AddSwaggerGen(options =>
        {
            var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
        }).AddSwaggerGenNewtonsoftSupport();

        services.Configure<FormOptions>(opt =>
        {
            //1GB Size Limit TODO: Move to configuration
            opt.ValueLengthLimit = 1048576000;
            opt.MultipartBodyLengthLimit = 1048576000;
            opt.MultipartHeadersLengthLimit = 1048576000;
        });


        return services;
    }

    /// <summary>
    ///     Configure the application
    /// </summary>
    /// <param name="app"></param>
    /// <returns></returns>
    public static WebApplication ConfigureApp(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        // // Get the File System from File System abstraction
        // var fileSystem = app.Services.GetRequiredService<IFileSystem>();

        app.UseCors(corsPolicyBuilder =>
            corsPolicyBuilder
                .AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod());
        app.UseMiddleware<ZipFileMiddleware>(new ZipFileMiddlewareOptions
        {
            RootPath = "wwwroot",
            ZipFileExtensions = new[] {".h5p"}
        });
        app.UseMiddleware<DefaultContentTypeMiddleware>();
        app.UseStaticFiles(new StaticFileOptions
        {
            ServeUnknownFileTypes = true
        });
        app.MapControllers();


        return app;
    }

    /// <summary>
    ///     Configure the webserver for production
    /// </summary>
    /// <param name="webApplicationBuilder"></param>
    /// <returns></returns>
    public static WebApplicationBuilder ConfigureWebserverForProduction(
        this WebApplicationBuilder webApplicationBuilder)
    {
        var config = webApplicationBuilder.Configuration.Get<BackendConfig>();

        webApplicationBuilder.WebHost.ConfigureKestrel(options => { options.ListenAnyIP(config!.HttpPort); });

        return webApplicationBuilder;
    }

    /// <summary>
    ///     Add logging to the application
    /// </summary>
    /// <param name="webApplicationBuilder"></param>
    /// <returns></returns>
    public static WebApplicationBuilder AddLogging(this WebApplicationBuilder webApplicationBuilder)
    {
        webApplicationBuilder.Logging.ClearProviders();
        webApplicationBuilder.Logging.AddConsole();

        return webApplicationBuilder;
    }
}