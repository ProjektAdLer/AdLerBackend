using System.IO.Abstractions;
using System.IO.Compression;
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

    public List<string>? StoreH5PFilesForWorld(WorldStoreH5PDto worldToStoreH5P)
    {
        var workingDir = _fileSystem.Path.Join("wwwroot", "courses", worldToStoreH5P.AuthorId.ToString(),
            worldToStoreH5P.WorldInforamtion.World.LmsElementIdentifier.Value, "h5p");

        var h5PFilePaths = worldToStoreH5P.H5PFiles.Select(item =>
        {
            var zipStream = new ZipArchive(item.H5PFile!, ZipArchiveMode.Read);

            var directory = _fileSystem.Path.Combine(workingDir, item.H5PFileName!);

            ExtractToDirectory(zipStream, directory);

            return directory;
        }).ToList();


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

    private void ExtractToDirectory(ZipArchive zipStream, string workingPath)
    {
        foreach (var entry in zipStream.Entries)
        {
            if (_fileSystem.Path.EndsInDirectorySeparator(entry.FullName)) continue;
            using var inputStream = entry.Open();

            var filePath = _fileSystem.Path.Join(workingPath, entry.FullName);
            var dirName = _fileSystem.Path.GetDirectoryName(filePath);

            _fileSystem.Directory.CreateDirectory(dirName.Trim());
            using var unpackedFile = _fileSystem.File.OpenWrite(filePath.Trim());
            inputStream.CopyTo(unpackedFile);
            unpackedFile.Flush();
        }
    }
}