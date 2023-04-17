using JetBrains.Annotations;

namespace AdLerBackend.Domain.Entities;

public class H5PLocationEntity : IBaseEntity
{
#pragma warning disable CS8618
    /// <summary>
    ///     This Empty Constructor is needed for EntityFramework
    ///     see https://docs.microsoft.com/en-us/ef/core/modeling/constructors
    /// </summary>
    [UsedImplicitly]
    public H5PLocationEntity()
    {
    }
#pragma warning restore CS8618

    public H5PLocationEntity(string path, int elementId, int? id)
    {
        Path = path;
        ElementId = elementId;
        Id = id;
    }

    public string Path { get; set; }
    public int ElementId { get; set; }
    public int? Id { get; set; }
}