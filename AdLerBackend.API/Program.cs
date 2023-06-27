using AdLerBackend.API;
using AdLerBackend.API.Properties;
using AdLerBackend.Application;
using AdLerBackend.Infrastructure;

// This is needed, because wwwroot directory must be present in the beginning to serve files from it
Directory.CreateDirectory("wwwroot");

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAndValidateBackendConfig(builder.Configuration);

builder.Services
    .AddApiServices()
    .AddApplicationServices()
    .AddInfrastructureServices(builder.Environment.IsDevelopment())
    .AddCors()
    .AddLogging();


if (!builder.Environment.IsDevelopment())
    builder.ConfigureWebserverForProduction();

var app = builder.Build();

var myConfig = app.Services.GetRequiredService<BackendConfig>();

app.Logger.LogInformation("Configuration: \n {MyConfig}", myConfig);

app
    .ConfigureApp()
    .Run();