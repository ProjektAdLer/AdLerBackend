// using System.IO.Abstractions;
// using System.IO.Abstractions.TestingHelpers;
// using System.IO.Compression;
// using AdLerBackend.API.Middleware;
// using Microsoft.AspNetCore.Http;
// using NSubstitute;
//
// namespace AdLerBackend.API.UnitTests.Middleware;
//
// public class UnzipMiddlewareTest
// {
//     private IFileSystem _fileSystem;
//     private RequestDelegate _next;
//
//     [SetUp]
//     public void Setup()
//     {
//         _next = Substitute.For<RequestDelegate>();
//         _fileSystem = new MockFileSystem();
//     }
//
//     [Test]
//     public async Task Invoke_UnzipsFile()
//     {
//         // Arrange
//         var context = new DefaultHttpContext
//         {
//             Request =
//             {
//                 Path = "/test.h5p/content.json"
//             }
//         };
//
//         var expectedContent = "test";
//         var memoryStream = CreateZipFileWithContentJson(expectedContent);
//
//         var memoryStreamBytes = memoryStream.ToArray();
//
//         // Create mock file system
//         var mockFileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
//         {
//             {"wwwroot/test.h5p", new MockFileData(memoryStreamBytes)}
//         });
//
//         var middleware = new UnzipMiddleware(_next, mockFileSystem);
//
//         // Act
//         await middleware.Invoke(context);
//
//         // Assert
//         Assert.That(context.Response.StatusCode, Is.EqualTo(200));
//         Assert.That(context.Response.ContentType, Is.EqualTo("application/json"));
//     }
//
//     [Test]
//     public async Task Invoke_CallsNext_WhenNoH5PInPath()
//     {
//         // Arrange
//         var context = new DefaultHttpContext
//         {
//             Request =
//             {
//                 Path = "/test/content.json"
//             }
//         };
//
//         var middleware = new UnzipMiddleware(_next, _fileSystem);
//
//         // Act
//         await middleware.Invoke(context);
//
//         // Assert
//         await _next.Received(1).Invoke(context);
//     }
//
//     [Test]
//     public async Task Invoke_Returns404_IfFileNotFound()
//     {
//         // Arrange
//         var context = new DefaultHttpContext
//         {
//             Request =
//             {
//                 Path = "/test.h5p/content2.json"
//             }
//         };
//
//         var expectedContent = "test";
//         var memoryStream = CreateZipFileWithContentJson(expectedContent);
//
//         var memoryStreamBytes = memoryStream.ToArray();
//
//         // Create mock file system
//         var mockFileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
//         {
//             {"wwwroot/test.h5p", new MockFileData(memoryStreamBytes)}
//         });
//
//         var middleware = new UnzipMiddleware(_next, mockFileSystem);
//
//         // Act
//         await middleware.Invoke(context);
//
//         // Assert
//         Assert.That(context.Response.StatusCode, Is.EqualTo(404));
//     }
//
//     public static MemoryStream CreateZipFileWithContentJson(string content)
//     {
//         var memoryStream = new MemoryStream();
//
//         using var archive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true);
//         var contentJsonEntry = archive.CreateEntry("content.json");
//         using var writer = new StreamWriter(contentJsonEntry.Open());
//         writer.Write(content);
//
//         memoryStream.Seek(0, SeekOrigin.Begin);
//         return memoryStream;
//     }
// }

