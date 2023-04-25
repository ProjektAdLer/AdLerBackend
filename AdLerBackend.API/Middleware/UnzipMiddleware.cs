using System.IO.Abstractions;
using System.IO.Compression;
using MimeKit;

namespace AdLerBackend.API.Middleware;

/// <summary>
///     Middleware to unzip files from a ZIP archive
/// </summary>
public class UnzipMiddleware
{
    private readonly IFileSystem _fileSystem;
    private readonly RequestDelegate _next;

    /// <summary>
    ///     Constructor
    /// </summary>
    /// <param name="next"></param>
    public UnzipMiddleware(RequestDelegate next, IFileSystem fileSystem)
    {
        _next = next;
        _fileSystem = fileSystem;
    }

    /// <summary>
    ///     Invoke the middleware
    /// </summary>
    /// <param name="context"></param>
    public async Task Invoke(HttpContext context)
    {
        var filePath = context.Request.Path.Value!.TrimStart('/');

        if (filePath.Contains(".h5p/"))
        {
            // Everything after the .h5p/ is the path inside the ZIP file
            // So everything leading to it is the path to the zip file
            var pathToZipFile = filePath.Substring(0, filePath.IndexOf(".h5p/", StringComparison.Ordinal)) + ".h5p";
            var pathInsideZipFile = filePath.Substring(filePath.IndexOf(".h5p/", StringComparison.Ordinal) + 5);
            using var zipFile =
                new ZipArchive(_fileSystem.File.OpenRead(_fileSystem.Path.Combine("wwwroot", pathToZipFile)),
                    ZipArchiveMode.Read);
            // use the Method from testable file system to open the zip file
            // Iterate through the entries in the ZIP file and search for the requested file
            foreach (var entry in zipFile.Entries)
                if (entry.FullName == pathInsideZipFile)
                {
                    // Found the requested file, so return it
                    // Get Actual content type by its file extension
                    // This is needed for the H5P player to work
                    var actualContentType = MimeTypes.GetMimeType(entry.Name);
                    context.Response.ContentType = actualContentType;
                    await entry.Open().CopyToAsync(context.Response.Body);
                    return;
                }

            // return with 404 if the requested file was not found
            context.Response.StatusCode = 404;
        }
        else
            // Pass the request through to the next middleware component
        {
            await _next.Invoke(context);
        }
    }
}