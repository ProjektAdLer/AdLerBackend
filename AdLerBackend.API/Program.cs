using AdLerBackend.API;
using AdLerBackend.Application;
using AdLerBackend.Application.Configuration;
using AdLerBackend.Infrastructure;

Directory.CreateDirectory("wwwroot");

var builder = WebApplication.CreateBuilder(args);

builder
    .AddLoggingViaSerilog()
    .Services.AddAndValidateBackendConfig(builder.Configuration);

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