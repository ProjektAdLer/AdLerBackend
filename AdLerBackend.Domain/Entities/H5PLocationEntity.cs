using JetBrains.Annotations;

namespace AdLerBackend.Domain.Entities;

public class H5PLocationEntity(string path, int? elementId = null, int? id = null) : IBaseEntity
{
    /// <summary>
    ///     This Empty Constructor is needed for EntityFramework as well as for AutoMapper.
    ///     see https://docs.microsoft.com/en-us/ef/core/modeling/constructors
    /// </summary>
    [UsedImplicitly]
    private H5PLocationEntity() : this(string.Empty, 0)
    {
    }

    public string Path { get; set; } = path;
    public int? ElementId { get; set; } = elementId;
    public int? Id { get; set; } = id;
}