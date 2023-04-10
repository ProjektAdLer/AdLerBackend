using System.Reflection;
using AdLerBackend.API.Filters;
using AdLerBackend.Application;
using AdLerBackend.Infrastructure;
using Microsoft.AspNetCore.Http.Features;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;


// This is needed, because wwwroot directory must be present in the beginning to serve files from it
Directory.CreateDirectory("wwwroot");


var builder = WebApplication.CreateBuilder(args);

// If the config file does not exist, create it
if (!File.Exists("./config/config.json"))
{
    File.WriteAllText("./config/config.json", JsonConvert.SerializeObject(new
    {
        useHttps = "false",
        httpPort = 80,
        moodleUrl = "Please specify moodle url"
    }, Formatting.Indented));

    // shut down program with message in dialog
    Console.WriteLine("Please edit the config file in ./config/config.json and restart the program.");
    Environment.Exit(1);
}

// Use Global AdLer Config File (Most likely coming from a docker volume)
builder.Configuration.AddJsonFile("./config/config.json", false);

// Add HTTPS support
if (!builder.Environment.IsDevelopment())
    builder.WebHost.ConfigureKestrel(options =>
    {
        if (builder.Configuration["useHttps"]?.ToLower() == "true")
            options.ListenAnyIP(int.Parse(builder.Configuration["httpsPort"] ?? "433"),
                listenOptions =>
                {
                    listenOptions.UseHttps("./config/cert/AdLerBackend.pfx",
                        builder.Configuration["httpsCertificatePassword"]);
                });
        else
            // if builder.Configuration["httpPort"] is not set, use default port 80
            options.ListenAnyIP(int.Parse(builder.Configuration["httpPort"] ?? "80"));
    });


builder.Services.AddControllers(
    options => { options.Filters.Add(new ApiExceptionFilterAttribute()); }
).AddNewtonsoftJson(opts =>
{
    // This converts enum integers to its corresponding string value
    opts.SerializerSettings.Converters.Add(new StringEnumConverter());
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
}).AddSwaggerGenNewtonsoftSupport();

builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration, builder.Environment.IsDevelopment());
builder.Services.Configure<FormOptions>(opt =>
{
    //1GB
    opt.ValueLengthLimit = 1048576000;
    opt.MultipartBodyLengthLimit = 1048576000;
    opt.MultipartHeadersLengthLimit = 1048576000;
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowEverything",
        policy => { policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod(); });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowEverything");

// Disabled for now, because it is not needed
//app.UseHttpsRedirection();

app.UseStaticFiles();

app.MapControllers();

app.Run();