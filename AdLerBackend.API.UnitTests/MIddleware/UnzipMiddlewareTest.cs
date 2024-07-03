using System.IO.Abstractions.TestingHelpers;
using System.IO.Compression;
using System.Text;
using AdLerBackend.API.Middleware;
using Microsoft.AspNetCore.Http;
using NSubstitute;

namespace AdLerBackend.API.UnitTests.MIddleware;

public class UnzipMiddlewareTest
{
    [TestFixture]
    public class UnzipMiddlewareTests
    {
        private MockFileSystem _mockFileSystem;
        private UnzipMiddleware _middleware;
        private RequestDelegate _nextDelegate;

        [SetUp]
        public void Setup()
        {
            _mockFileSystem = new MockFileSystem();
            _nextDelegate = Substitute.For<RequestDelegate>();
            _middleware = new UnzipMiddleware(_nextDelegate, _mockFileSystem);
        }

        [Test]
        // ANF-ID: [BPG21]
        public async Task Invoke_WithNonH5pPath_CallsNextMiddleware()
        {
            // Arrange
            var context = new DefaultHttpContext();
            context.Request.Path = "/some/other/path";

            // Act
            await _middleware.Invoke(context);

            // Assert
            await _nextDelegate.Received(1).Invoke(context);
        }

        // ANF-ID: [BPG21]
        [Test]
        public async Task Invoke_WithH5pPath_FileFound_ReturnsFileContent()
        {
            var expectedContent = "console.log('Hello World');";
            // Arrange
            var context = new DefaultHttpContext();
            context.Request.Path = "/path/to/file.h5p/content/file.js";
            var responseStream = new MemoryStream();
            context.Response.Body = responseStream;

            // Create a mock ZIP file
            var zipStream = new MemoryStream();
            using (var zipArchive = new ZipArchive(zipStream, ZipArchiveMode.Create, true))
            {
                var entry = zipArchive.CreateEntry("content/file.js");
                using var entryStream = entry.Open();
                var content = Encoding.UTF8.GetBytes(expectedContent);
                entryStream.Write(content, 0, content.Length);
            }
            zipStream.Seek(0, SeekOrigin.Begin);

            _mockFileSystem.AddFile("/wwwroot/path/to/file.h5p", new MockFileData(zipStream.ToArray()));

            // Act
            await _middleware.Invoke(context);

            // Assert
            responseStream.Seek(0, SeekOrigin.Begin);
            var reader = new StreamReader(responseStream);
            var responseContent = await reader.ReadToEndAsync();
            Assert.That(responseContent, Is.EqualTo(expectedContent));
        }

        // ANF-ID: [BPG21]
        [Test]
        public async Task Invoke_WithH5pPath_FileNotFound_Returns404()
        {
            // Arrange
            var context = new DefaultHttpContext();
            context.Request.Path = "/path/to/file.h5p/content/nonexistent.js";
        
            // Create a mock ZIP file without the requested file
            var zipStream = new MemoryStream();
            using (var zipArchive = new ZipArchive(zipStream, ZipArchiveMode.Create, true))
            {
                zipArchive.CreateEntry("content/other.js");
            }
            zipStream.Seek(0, SeekOrigin.Begin);
        
            _mockFileSystem.AddFile("/wwwroot/path/to/file.h5p", new MockFileData(zipStream.ToArray()));
        
            // Act
            await _middleware.Invoke(context);
        
            // Assert
            Assert.That(context.Response.StatusCode, Is.EqualTo(404));
            Assert.That(context.Response.Body.Length, Is.EqualTo(0));
        }
        
        // ANF-ID: [BPG21]
        [Test]
        public async Task Invoke_PathNotContainingH5p_CallsNextMiddleware()
        {
            // Arrange
            var context = new DefaultHttpContext();
            context.Request.Path = "/path/to/file.js";
        
            // Act
            await _middleware.Invoke(context);
        
            // Assert
            await _nextDelegate.Received(1).Invoke(context);
        }
    }
}