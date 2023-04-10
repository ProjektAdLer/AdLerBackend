using AdLerBackend.Application.Common.DTOs.Storage;

namespace AdLerBackend.Application.Common.Interfaces;

public interface IFileAccess
{
    public List<string>? StoreH5PFilesForWorld(WorldStoreH5PDto worldToStoreH5P);
    public string StoreAtfFileForWorld(StoreWorldAtfDto worldToStoreH5P);
    public Stream GetFileStream(string filePath);
    public string StoreH5PBase(Stream fileStream);
    public bool DeleteWorld(WorldDeleteDto worldToDelete);
}