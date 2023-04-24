using AdLerBackend.Application.Common.DTOs.Storage;

namespace AdLerBackend.Application.Common.Interfaces;

public interface IFileAccess
{
    public List<string>? StoreH5PFilesForWorld(WorldStoreH5PDto worldToStoreH5P);
    public string StoreAtfFileForWorld(StoreWorldAtfDto worldToStoreH5P);
    public Stream GetReadFileStream(string filePath);
    public bool DeleteWorld(WorldDeleteDto worldToDelete);
}