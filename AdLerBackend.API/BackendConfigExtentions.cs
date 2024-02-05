using System.ComponentModel.DataAnnotations;
using System.Configuration;
using AdLerBackend.Application.Configuration;
using Microsoft.Extensions.Options;
using Serilog;
using Serilog.Settings.Configuration;
using Serilog.Sinks.SystemConsole.Themes;

namespace AdLerBackend.API;

public static class BackendConfigExtensions
{
    public static WebApplicationBuilder AddLoggingViaSerilog(this WebApplicationBuilder builder)
    {
        var options = new ConfigurationReaderOptions(typeof(ConsoleLoggerExtensions).Assembly);
        var loggerConfig = new LoggerConfiguration();
        loggerConfig.ReadFrom.Configuration(builder.Configuration, options)
            .Enrich.FromLogContext()
            .WriteTo.Console(
                outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}",
                theme: AnsiConsoleTheme.Code
            );

        Log.Logger = loggerConfig.CreateLogger();

        // clear providers and add Serilog
        builder.Logging.ClearProviders();
        builder.Logging.AddSerilog(Log.Logger);

        return builder;
    }

    public static IServiceCollection AddAndValidateBackendConfig(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<BackendConfig>(configuration);

        services.PostConfigure<BackendConfig>(myConfig =>
        {
            var context = new ValidationContext(myConfig);
            var results = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(myConfig, context, results, true);

            if (!isValid)
                throw new ConfigurationErrorsException("Configuration validation failed: " +
                                                       string.Join(", ", results.Select(x => x.ErrorMessage)));
        });

        services.AddSingleton(resolver => resolver.GetRequiredService<IOptions<BackendConfig>>().Value);

        return services;
    }
}