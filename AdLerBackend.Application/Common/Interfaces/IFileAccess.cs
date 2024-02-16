using AdLerBackend.Application.Common.DTOs.Storage;

namespace AdLerBackend.Application.Common.Interfaces;

public interface IFileAccess
{
    /// <summary>
    /// Stores the H5P files in wwwRoot. Returns a dictionary with the H5P UUID as key and the file path as value.
    /// </summary>
    /// <param name="worldToStoreH5P"></param>
    /// <returns></returns>
    public Dictionary<string, string>? StoreH5PFilesForWorld(WorldStoreH5PDto worldToStoreH5P);
    public bool DeleteWorld(WorldDeleteDto worldToDelete);
}