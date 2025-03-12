using System.Net.Mime;
using System.Text.Json;
using AdLerBackend.API;
using AdLerBackend.Application;
using AdLerBackend.Application.Configuration;
using AdLerBackend.Infrastructure;
using AdLerBackend.Infrastructure.Repositories.BaseContext;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;

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

// Apply migrations
if (!builder.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<BaseAdLerBackendDbContext>();

    dbContext.Database.Migrate();
}

// Configure the health check endpoint with detailed information
app.MapHealthChecks("/api/health", new HealthCheckOptions
{
    ResponseWriter = async (context, report) =>
    {
        var result = new
        {
            status = report.Status.ToString(),
            checks = report.Entries.Select(entry => new
            {
                name = entry.Key,
                status = entry.Value.Status.ToString(),
                description = entry.Value.Description,
                exception = entry.Value.Exception?.Message,
                duration = entry.Value.Duration.ToString()
            })
        };

        context.Response.ContentType = MediaTypeNames.Application.Json;
        await context.Response.WriteAsync(JsonSerializer.Serialize(result));
    }
});

var myConfig = app.Services.GetRequiredService<BackendConfig>();

app.Logger.LogInformation("Application started with the following configuration: {@MyConfig:n}", myConfig);

app
    .ConfigureApp()
    .Run();