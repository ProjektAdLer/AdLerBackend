using System.ComponentModel.DataAnnotations;
using AdLerBackend.API;
using AdLerBackend.API.Properties;
using AdLerBackend.Application;
using AdLerBackend.Infrastructure;

// This is needed, because wwwroot directory must be present in the beginning to serve files from it
Directory.CreateDirectory("wwwroot");

var builder = WebApplication.CreateBuilder(args);


var configuration = new ConfigurationBuilder()
    .AddEnvironmentVariables()
    .Build();

builder.Services.Configure<BackendConfig>(configuration);

// get the configuration object
var myConfig = builder.Configuration.Get<BackendConfig>();


// Validate the BackendConfig object
var context = new ValidationContext(myConfig);
var results = new List<ValidationResult>();

var isValid = Validator.TryValidateObject(myConfig, context, results, true);


if (!isValid)
    throw new Exception("Configuration validation failed: " + string.Join(", ", results.Select(x => x.ErrorMessage)));

builder.Services
    .AddApiServices()
    .AddApplicationServices()
    .AddInfrastructureServices(builder.Environment.IsDevelopment())
    .AddCors()
    .AddLogging();

if (!builder.Environment.IsDevelopment())
    builder.ConfigureWebserverForProduction();

var app = builder.Build();

app.Logger.LogInformation("Configuration: \n" + myConfig);

app
    .ConfigureApp()
    .Run();