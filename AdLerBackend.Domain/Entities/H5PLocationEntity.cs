using AdLerBackend.Domain.Entities.Common;

namespace AdLerBackend.Domain.Entities;

public class H5PLocationEntity : BaseEntity
{
    public string Path { get; set; }
    public int ElementId { get; set; }
}