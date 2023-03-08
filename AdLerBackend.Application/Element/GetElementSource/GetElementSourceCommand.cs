using AdLerBackend.Application.Common;
using AdLerBackend.Application.Common.Responses.Elements;

namespace AdLerBackend.Application.Element.GetElementSource;

public record GetElementSourceCommand : CommandWithToken<GetElementSourceResponse>
{
    public int ElementId { get; init; }
    public int WorldId { get; init; }
}