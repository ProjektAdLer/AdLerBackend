using System.IO.Abstractions;
using AdLerBackend.Application.Common.DTOs.Storage;
using AdLerBackend.Application.Common.Interfaces;
using Microsoft.Extensions.Logging;

namespace AdLerBackend.Infrastructure.Storage;

public class StorageService(IFileSystem fileSystem, ILogger<StorageService> logger) : IFileAccess
{
    public Dictionary<string, string>? StoreH5PFilesForWorld(WorldStoreH5PDto worldToStoreH5P)
    {
        var workingDir = fileSystem.Path.Join("wwwroot", "courses", worldToStoreH5P.CourseInstanceId.ToString(),
            "h5p");

        // create directory if not exists
        if (!fileSystem.Directory.Exists(workingDir))
            fileSystem.Directory.CreateDirectory(workingDir);

        var h5PFilePaths = new Dictionary<string, string>();

        foreach (var h5PFile in worldToStoreH5P.H5PFiles)
        {
            var h5PFilePath = fileSystem.Path.Combine(workingDir, h5PFile.H5PUuid + ".h5p");

            var fileStream = fileSystem.File.Create(h5PFilePath);
            h5PFile.H5PFile!.CopyTo(fileStream);
            fileStream.Close();

            h5PFilePaths.Add(h5PFile.H5PUuid!, h5PFilePath);
        }


        return h5PFilePaths;
    }

    public bool DeleteWorld(WorldDeleteDto worldToDelete)
    {
        var workingDir = fileSystem.Path.Join("wwwroot", "courses", worldToDelete.WorldInstanceId.ToString());

        if (!fileSystem.Directory.Exists(workingDir))
        {
            logger.LogWarning($"World with ID {worldToDelete.WorldInstanceId} does not exist.");
            return true;
        }

        fileSystem.Directory.Delete(workingDir, true);
        return true;
    }
}