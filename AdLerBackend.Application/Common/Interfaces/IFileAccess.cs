using AdLerBackend.Application.Common.DTOs.Storage;

namespace AdLerBackend.Application.Common.Interfaces;

public interface IFileAccess
{
    public Dictionary<string, string>? StoreH5PFilesForWorld(WorldStoreH5PDto worldToStoreH5P);
    public bool DeleteWorld(WorldDeleteDto worldToDelete);
}