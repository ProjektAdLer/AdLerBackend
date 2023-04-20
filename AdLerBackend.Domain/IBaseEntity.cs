using JetBrains.Annotations;

namespace AdLerBackend.Domain;

public interface IBaseEntity
{
    [UsedImplicitly] public int? Id { get; set; }
}