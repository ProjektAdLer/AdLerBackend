using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using System.IO.Compression;
using AdLerBackend.API.Middleware;
using Microsoft.AspNetCore.Http;
using NSubstitute;

namespace AdLerBackend.API.UnitTests.Middleware;

public class UnzipMiddlewareTests
{
    private IFileSystem _fileSystem;
    private HttpContext _httpContext;
    private RequestDelegate _nextDelegate;
    private MemoryStream _responseStream;

    [SetUp]
    public void Setup()
    {
        _fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
        {
            {@"wwwroot\test.h5p", CreateSampleZip()}
        });

        _httpContext = new DefaultHttpContext();

        _responseStream = new MemoryStream();
        _httpContext.Response.Body = _responseStream;

        _nextDelegate = Substitute.For<RequestDelegate>();
    }

    [Test]
    public async Task Invoke_ValidPathInsideZipFile_ReturnsFileContent()
    {
        _httpContext.Request.Path = "/test.h5p/sample.txt";

        var middleware = new UnzipMiddleware(_nextDelegate, _fileSystem);
        await middleware.Invoke(_httpContext);

        Assert.That(_httpContext.Response.StatusCode, Is.EqualTo(200));
        _responseStream.Position = 0;
        using var reader = new StreamReader(_responseStream);
        var content = await reader.ReadToEndAsync();
        Assert.That(content, Is.EqualTo("Sample content"));
    }

    [Test]
    public async Task Invoke_PathNotFoundInsideZipFile_Returns404()
    {
        _httpContext.Request.Path = "/test.h5p/notfound.txt";

        var middleware = new UnzipMiddleware(_nextDelegate, _fileSystem);
        await middleware.Invoke(_httpContext);

        Assert.That(_httpContext.Response.StatusCode, Is.EqualTo(404));
    }

    [Test]
    public async Task Invoke_NoneZipPath_PassesThroughToNextMiddleware()
    {
        _httpContext.Request.Path = "/notrelated.txt";

        var middleware = new UnzipMiddleware(_nextDelegate, _fileSystem);
        await middleware.Invoke(_httpContext);

        await _nextDelegate.Received()(Arg.Is<HttpContext>(ctx => ctx == _httpContext));
    }

    private static MockFileData CreateSampleZip()
    {
        using var memoryStream = new MemoryStream();
        using (var archive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
        {
            var sampleFile = archive.CreateEntry("sample.txt");
            using var writer = new StreamWriter(sampleFile.Open());
            writer.Write("Sample content");
        }

        return new MockFileData(memoryStream.ToArray());
    }
}