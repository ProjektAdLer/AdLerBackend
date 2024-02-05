// This is needed, because wwwroot directory must be present in the beginning to serve files from it

using AdLerBackend.API;
using AdLerBackend.Application;
using AdLerBackend.Application.Configuration;
using AdLerBackend.Infrastructure;
using Serilog;
using Serilog.Settings.Configuration;
using Serilog.Sinks.SystemConsole.Themes;

Directory.CreateDirectory("wwwroot");

var builder = WebApplication.CreateBuilder(args);


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


builder.Services.AddAndValidateBackendConfig(builder.Configuration);

builder.Services
    .AddApiServices()
    .AddApplicationServices()
    .AddInfrastructureServices(builder.Environment.IsDevelopment())
    .AddCors();


if (!builder.Environment.IsDevelopment())
    builder.ConfigureWebserverForProduction();

var app = builder.Build();

app.MapHealthChecks("/api/health");

var myConfig = app.Services.GetRequiredService<BackendConfig>();

app.Logger.LogInformation("Application started with the following configuration: {@MyConfig:n}", myConfig);

app
    .ConfigureApp()
    .Run();