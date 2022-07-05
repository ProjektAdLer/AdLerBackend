using Microsoft.Net.Http.Headers;
using System.IO.Compression;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "test",
                      policy =>
                      {
                          //policy.WithOrigins("http://example.com",
                          //                    "http://www.contoso.com",
                          //                    "http://localhost:3000");
                          policy.AllowAnyOrigin();
                          policy.WithHeaders(HeaderNames.AccessControlAllowHeaders, "*");
                      });
});
builder.Services.AddLogging();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseCors("test");

app.MapPost("/Files/UploadFiles", (HttpRequest request) =>
{
    if (!request.Form.Files.Any()) return Results.BadRequest("At least one file is needed");

    foreach (var file in request.Form.Files)
    {


        string fileName = Path.Join("wwwroot", "TestH5P", file.FileName);
        string rootPath = Path.ChangeExtension(fileName, null);
        Directory.CreateDirectory(rootPath);


        using (var zip = new ZipArchive(file.OpenReadStream(), ZipArchiveMode.Read))
        {
            foreach (var entry in zip.Entries)
            {
                using (Stream inputStream = entry.Open())
                {
                    string filePath = Path.Join(rootPath, entry.FullName);
                    string? dirName = Path.GetDirectoryName(filePath);

                    if (dirName == null) throw new Exception("dirName is null");

                    Directory.CreateDirectory(dirName);
                    using (var unpackedFile = File.OpenWrite(filePath))
                    {
                        inputStream.CopyTo(unpackedFile);
                        unpackedFile.Flush();
                    }

                }
            }
        }



    }



    return Results.Ok();
});


app.UseStaticFiles();

app.MapControllers();

app.Run();
