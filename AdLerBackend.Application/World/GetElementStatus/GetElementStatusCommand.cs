using AdLerBackend.Application.Common;
using AdLerBackend.Application.Common.Responses.Course;

namespace AdLerBackend.Application.World.GetElementStatus;

public record GetElementStatusCommand : CommandWithToken<ElementStatusResponse>
{
    public int WorldId { get; init; }
}