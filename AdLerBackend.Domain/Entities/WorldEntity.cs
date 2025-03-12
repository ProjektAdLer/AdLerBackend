using JetBrains.Annotations;

namespace AdLerBackend.Domain.Entities;

public class WorldEntity(
    string name,
    List<H5PLocationEntity> h5PFilesInCourse,
    int authorId,
    string atfJson,
    int lmsWorldId,
    int? id = null)
    : IBaseEntity
{
    /// <summary>
    ///     This Empty Constructor is needed for EntityFramework as well as for AutoMapper.
    ///     see https://docs.microsoft.com/en-us/ef/core/modeling/constructors
    ///     (EF Core cannot set navigation properties using a constructor.)
    /// </summary>
    [UsedImplicitly]
    internal WorldEntity() : this("", new List<H5PLocationEntity>(), 0, "", 0)
    {
        // Initialize every property with a default value
    }


    public string Name { get; set; } = name;
    public int LmsWorldId { get; set; } = lmsWorldId;
    public List<H5PLocationEntity> H5PFilesInCourse { get; set; } = h5PFilesInCourse;

    public int AuthorId { get; set; } = authorId;

    // This is a very long string
    public string AtfJson { get; set; } = atfJson;
    public int? Id { get; set; } = id;
}