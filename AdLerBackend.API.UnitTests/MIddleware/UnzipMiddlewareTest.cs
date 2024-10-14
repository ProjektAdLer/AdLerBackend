using System.IO.Abstractions.TestingHelpers;
using System.IO.Compression;
using System.Text;
using AdLerBackend.API.Middleware;
using FluentAssertions;
using Microsoft.AspNetCore.Http;

namespace AdLerBackend.API.UnitTests.MIddleware;

[TestFixture]
public class ZipFileMiddlewareTests
{
    [SetUp]
    public void SetUp()
    {
        _mockFileSystem = new MockFileSystem();
        _options = new ZipFileMiddlewareOptions
        {
            ZipFileExtensions = new[] {".zip"},
            RootPath = "/wwwroot"
        };
        _nextDelegate = context => Task.CompletedTask;
        _middleware = new ZipFileMiddleware(_nextDelegate, _mockFileSystem, _options);
    }

    private MockFileSystem _mockFileSystem;
    private ZipFileMiddlewareOptions _options;
    private RequestDelegate _nextDelegate;
    private ZipFileMiddleware _middleware;

    [Test]
    public async Task Invoke_WithNonZipPath_CallsNextMiddleware()
    {
        // Arrange
        var context = new DefaultHttpContext();
        context.Request.Path = "/some/other/path";

        // Act
        await _middleware.Invoke(context);

        // Assert
        // Since _nextDelegate does nothing, we can assert that the response is unmodified
        context.Response.StatusCode.Should().Be(200); // Default status code
    }

    [Test]
    public async Task Invoke_WithZipPath_FileFound_ReturnsFileContent()
    {
        // Arrange
        var zipFileName = "archive.zip";
        var entryName = "content/file.js";
        var entryContent = "console.log('Hello World');";
        var fullZipFilePath = _mockFileSystem.Path.Combine(_options.RootPath, zipFileName);

        // Create a mock ZIP file
        var zipStream = new MemoryStream();
        using (var zipArchive = new ZipArchive(zipStream, ZipArchiveMode.Create, true))
        {
            var entry = zipArchive.CreateEntry(entryName);
            using var entryStream = entry.Open();
            var contentBytes = Encoding.UTF8.GetBytes(entryContent);
            entryStream.Write(contentBytes, 0, contentBytes.Length);
        }

        zipStream.Seek(0, SeekOrigin.Begin);

        _mockFileSystem.AddFile(fullZipFilePath, new MockFileData(zipStream.ToArray()));

        var context = new DefaultHttpContext();
        context.Request.Path = $"/{zipFileName}/{entryName}";
        context.Response.Body = new MemoryStream();

        // Act
        await _middleware.Invoke(context);

        // Assert
        context.Response.StatusCode.Should().Be(200);
        context.Response.ContentType.Should().Be("text/javascript");
        context.Response.Body.Seek(0, SeekOrigin.Begin);
        using var reader = new StreamReader(context.Response.Body);
        var responseContent = await reader.ReadToEndAsync();
        responseContent.Should().Be(entryContent);
    }

    [Test]
    public async Task Invoke_WithZipPath_FileNotFoundInZip_Returns404()
    {
        // Arrange
        var zipFileName = "archive.zip";
        var entryName = "content/nonexistent.js";
        var fullZipFilePath = _mockFileSystem.Path.Combine(_options.RootPath, zipFileName);

        // Create a mock ZIP file without the requested file
        var zipStream = new MemoryStream();
        using (var zipArchive = new ZipArchive(zipStream, ZipArchiveMode.Create, true))
        {
            zipArchive.CreateEntry("content/other.js");
        }

        zipStream.Seek(0, SeekOrigin.Begin);

        _mockFileSystem.AddFile(fullZipFilePath, new MockFileData(zipStream.ToArray()));

        var context = new DefaultHttpContext();
        context.Request.Path = $"/{zipFileName}/{entryName}";
        context.Response.Body = new MemoryStream();

        // Act
        await _middleware.Invoke(context);

        // Assert
        context.Response.StatusCode.Should().Be(404);
        context.Response.Body.Length.Should().Be(0);
    }

    [Test]
    public async Task Invoke_WithZipPath_ZipFileNotFound_Returns404()
    {
        // Arrange
        var zipFileName = "nonexistent.zip";
        var entryName = "content/file.js";
        var fullZipFilePath = _mockFileSystem.Path.Combine(_options.RootPath, zipFileName);

        // Ensure the zip file does not exist
        _mockFileSystem.File.Exists(fullZipFilePath).Should().BeFalse();

        var context = new DefaultHttpContext();
        context.Request.Path = $"/{zipFileName}/{entryName}";
        context.Response.Body = new MemoryStream();

        // Act
        await _middleware.Invoke(context);

        // Assert
        context.Response.StatusCode.Should().Be(404);
        context.Response.Body.Length.Should().Be(0);
    }

    [Test]
    public async Task Invoke_ExceptionThrown_Returns500()
    {
        // Arrange
        var zipFileName = "archive.zip";
        var entryName = "content/file.js";
        var fullZipFilePath = _mockFileSystem.Path.Combine(_options.RootPath, zipFileName);

        // Add a corrupted zip file to simulate an exception
        _mockFileSystem.AddFile(fullZipFilePath, new MockFileData(new byte[] {0x0}));

        var context = new DefaultHttpContext();
        context.Request.Path = $"/{zipFileName}/{entryName}";
        context.Response.Body = new MemoryStream();

        // Act
        await _middleware.Invoke(context);

        // Assert
        context.Response.StatusCode.Should().Be(500);
        context.Response.Body.Length.Should().Be(0);
    }

    [Test]
    public async Task Invoke_PathNotContainingZipExtension_CallsNextMiddleware()
    {
        // Arrange
        var context = new DefaultHttpContext();
        context.Request.Path = "/path/to/file.js";

        // Act
        await _middleware.Invoke(context);

        // Assert
        context.Response.StatusCode.Should().Be(200);
    }
}