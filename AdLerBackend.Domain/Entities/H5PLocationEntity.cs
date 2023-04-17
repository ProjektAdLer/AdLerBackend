using JetBrains.Annotations;

namespace AdLerBackend.Domain.Entities;

public class H5PLocationEntity : IBaseEntity
{
    /// <summary>
    ///     This Empty Constructor is needed for EntityFramework as well as for AutoMapper.
    ///     see https://docs.microsoft.com/en-us/ef/core/modeling/constructors
    /// </summary>
    [UsedImplicitly]
    private H5PLocationEntity()
    {
        Path = string.Empty;
        ElementId = 0;
        Id = null;
    }

    public H5PLocationEntity(string path, int? elementId = null, int? id = null)
    {
        Path = path;
        ElementId = elementId;
        Id = id;
    }

    public string Path { get; set; }
    public int? ElementId { get; set; }
    public int? Id { get; set; }
}