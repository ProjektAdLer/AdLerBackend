using AdLerBackend.Domain.Entities;
using JetBrains.Annotations;

namespace AdLerBackend.Domain.UnitTests.TestingUtils;

public class WorldEntityFactory
{
    public static WorldEntity CreateWorldEntity(
        [CanBeNull] string name = "Test World",
        [CanBeNull] List<H5PLocationEntity> h5PFilesInCourse = null,
        [CanBeNull] string dslLocation = "Test Location",
        int? authorId = 1,
        [CanBeNull] string atfJson = "Test Json",
        [CanBeNull] int lmsInstanceUuidId = 0,
        int? id = null)
    {
        return new
            WorldEntity(name, h5PFilesInCourse ?? new List<H5PLocationEntity>(), dslLocation, (int) authorId!,
                atfJson!,
                lmsInstanceUuidId, id);
    }
}