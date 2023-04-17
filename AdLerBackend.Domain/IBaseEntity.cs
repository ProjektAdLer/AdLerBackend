using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("AdLerBackend.Application.UnitTests")]
[assembly: InternalsVisibleTo("AdLerBackend.Domain.UnitTests")]
[assembly: InternalsVisibleTo("AdLerBackend.Infrastructure.UnitTests")]

namespace AdLerBackend.Domain;

public interface IBaseEntity
{
    public int? Id { get; init; }
}