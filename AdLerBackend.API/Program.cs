using System.Reflection;
using AdLerBackend.API.Filters;
using AdLerBackend.API.Middleware;
using AdLerBackend.Application;
using AdLerBackend.Infrastructure;
using Microsoft.AspNetCore.Http.Features;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace AdLerBackend.API;

internal static class Program
{
    public static void Main(string[] args)
    {
        if (!File.Exists("./config/config.json")) CreateDefaultConfigAndCrash();

        // This is needed, because wwwroot directory must be present in the beginning to serve files from it
        Directory.CreateDirectory("wwwroot");

        var builder = WebApplication.CreateBuilder(args);

        builder.Logging.ClearProviders();
        builder.Logging.AddConsole();

        // Use Global AdLer Config File (Most likely coming from a docker volume)
        builder.Configuration.AddJsonFile("./config/config.json", false);

        ConfigureHttpOrHttpsFromConfig(builder);


        builder.Services
            .AddControllers(options => options.Filters.Add<ApiExceptionFilterAttribute>())
            .AddNewtonsoftJson(opts => opts.SerializerSettings.Converters.Add(new StringEnumConverter()));


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
            //1GB Size Limit TODO: Move to configuration
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
        app.UseMiddleware<UnzipMiddleware>();
        app.UseStaticFiles(new StaticFileOptions
        {
            ServeUnknownFileTypes = true
        });

        app.MapControllers();

        app.Run();

        void CreateDefaultConfigAndCrash()
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

        void ConfigureHttpOrHttpsFromConfig(WebApplicationBuilder webApplicationBuilder)
        {
            if (!webApplicationBuilder.Environment.IsDevelopment())
                webApplicationBuilder.WebHost.ConfigureKestrel(options =>
                {
                    if (webApplicationBuilder.Configuration["useHttps"]?.ToLower() == "true")
                        options.ListenAnyIP(int.Parse(webApplicationBuilder.Configuration["httpsPort"] ?? "433"),
                            listenOptions =>
                            {
                                listenOptions.UseHttps("./config/cert/AdLerBackend.pfx",
                                    webApplicationBuilder.Configuration["httpsCertificatePassword"]);
                            });
                    else
                        // if builder.Configuration["httpPort"] is not set, use default port 80
                        options.ListenAnyIP(int.Parse(webApplicationBuilder.Configuration["httpPort"] ?? "80"));
                });
        }
    }
}