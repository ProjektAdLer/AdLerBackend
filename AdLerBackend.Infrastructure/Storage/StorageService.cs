using System.IO.Abstractions;
using AdLerBackend.Application.Common.DTOs.Storage;
using AdLerBackend.Application.Common.Exceptions;
using AdLerBackend.Application.Common.Interfaces;

namespace AdLerBackend.Infrastructure.Storage;

public class StorageService : IFileAccess
{
    private readonly IFileSystem _fileSystem;

    public StorageService(IFileSystem fileSystem)
    {
        _fileSystem = fileSystem;
    }

    public Dictionary<string, string>? StoreH5PFilesForWorld(WorldStoreH5PDto worldToStoreH5P)
    {
        var workingDir = _fileSystem.Path.Join("wwwroot", "courses", worldToStoreH5P.AuthorId.ToString(),
            worldToStoreH5P.WorldInforamtion.World.LmsElementIdentifier.Value, "h5p");

        // create directory if not exists
        if (!_fileSystem.Directory.Exists(workingDir))
            _fileSystem.Directory.CreateDirectory(workingDir);

        var h5PFilePaths = new Dictionary<string, string>();

        foreach (var h5PFile in worldToStoreH5P.H5PFiles)
        {
            var h5PFilePath = _fileSystem.Path.Combine(workingDir, h5PFile.H5PFileName + ".h5p");

            var fileStream = _fileSystem.FileStream.Create(h5PFilePath, FileMode.Create);
            h5PFile.H5PFile!.CopyTo(fileStream);
            fileStream.Close();

            h5PFilePaths.Add(h5PFile.H5PFileName!, h5PFilePath);
        }


        return h5PFilePaths;
    }

    public string StoreAtfFileForWorld(StoreWorldAtfDto worldToStoreH5P)
    {
        worldToStoreH5P.AtfFile.Position = 0;
        var workingDir = _fileSystem.Path.Join("wwwroot", "courses", worldToStoreH5P.AuthorId.ToString(),
            worldToStoreH5P.WorldInforamtion.World.LmsElementIdentifier.Value);

        // save stream on courseToStore on disk
        var dslFilePath = _fileSystem.Path.Combine(workingDir,
            worldToStoreH5P.WorldInforamtion.World.LmsElementIdentifier.Value + ".json");

        // create directory if not exists
        if (!_fileSystem.Directory.Exists(workingDir))
            _fileSystem.Directory.CreateDirectory(workingDir);

        var fileStream = _fileSystem.FileStream.Create(dslFilePath, FileMode.Create);
        worldToStoreH5P.AtfFile.CopyTo(fileStream);
        fileStream.Close();
        return dslFilePath;
    }

    public Stream GetReadFileStream(string filePath)
    {
        try
        {
            var fileStream = _fileSystem.FileStream.Create(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            return fileStream;
        }
        catch (Exception e)
        {
            throw new NotFoundException("File not found: " + filePath, e);
        }
    }

    public bool DeleteWorld(WorldDeleteDto worldToDelete)
    {
        var workingDir = _fileSystem.Path.Join("wwwroot", "courses", worldToDelete.AuthorId.ToString(),
            worldToDelete.WorldName);

        _fileSystem.Directory.Delete(workingDir, true);
        return true;
    }
}