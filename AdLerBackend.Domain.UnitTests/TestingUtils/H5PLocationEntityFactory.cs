using AdLerBackend.Domain.Entities;
using JetBrains.Annotations;

namespace AdLerBackend.Domain.UnitTests.TestingUtils;

public static class H5PLocationEntityFactory
{
    public static H5PLocationEntity CreateH5PLocationEntity(
        [CanBeNull] string path = "Test Path",
        int? elementId = null,
        int? id = null)
    {
        return new H5PLocationEntity(path!, elementId, id);
    }
}