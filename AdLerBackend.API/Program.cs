using System.Text;
using AdLerBackend.API;
using AdLerBackend.Application;
using AdLerBackend.Infrastructure;

// This is needed, because wwwroot directory must be present in the beginning to serve files from it
Directory.CreateDirectory("wwwroot");

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddApiServices()
    .AddApplicationServices()
    .AddInfrastructureServices(builder.Environment.IsDevelopment())
    .AddLogging();


if (!builder.Environment.IsDevelopment())
    builder.ConfigureWebserverForProduction();

var app = builder.Build();


var stringBuilder = new StringBuilder();

// crash when the Moodle url is not set
if (string.IsNullOrEmpty(app.Configuration["ASPNETCORE_ADLER_MOODLE_URL"]))
    throw new Exception("Moodle url is not set! \n please set it in appsettings.json or in environment variables");


if (builder.Environment.IsDevelopment())
    app.Logger.LogInformation("We are in development mode!");
else
    app.Logger.LogInformation("We are in production mode!");

stringBuilder.AppendLine("Key".PadRight(40) + "Value");
// add table rows
foreach (var (key, value) in app.Configuration.AsEnumerable())
    if (!string.IsNullOrEmpty(value) &&
        key.StartsWith(
            "ASPNETCORE_ADLER_")) // omit keys with null values and those that do not start with "ASPNETCORE_ADLER_"
    {
        // format row
        var row = key.PadRight(40) + value;
        stringBuilder.AppendLine(row);
    }

app.Logger.LogInformation("Configuration: \n" + stringBuilder);


app
    .ConfigureApp()
    .Run();