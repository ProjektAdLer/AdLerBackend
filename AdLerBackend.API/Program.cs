using AdLerBackend.API;
using AdLerBackend.Application;
using AdLerBackend.Infrastructure;

// This is needed, because wwwroot directory must be present in the beginning to serve files from it
Directory.CreateDirectory("wwwroot");

var builder = WebApplication.CreateBuilder(args);

// Use Global AdLer Config File (Most likely coming from a docker volume)
if (!File.Exists("./config/config.json"))
    ConfigurationLoader.CreateDefaultConfigAndCrash();

builder.Configuration.AddJsonFile("./config/config.json", false);


builder.Services
    .AddApiServices()
    .AddApplicationServices()
    .AddInfrastructureServices(builder.Environment.IsDevelopment())
    .AddLogging();


if (!builder.Environment.IsDevelopment())
    builder.ConfigureWebserverForProduction();


var app = builder.Build();

app
    .ConfigureApp()
    .Run();