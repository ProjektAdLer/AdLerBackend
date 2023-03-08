using AdLerBackend.Application.Common;
using AdLerBackend.Application.Common.Responses.Course;

namespace AdLerBackend.Application.World.GetWorldsForAuthor;

public record GetWorldsForAuthorCommand : CommandWithToken<GetWorldOverviewResponse>
{
    public int AuthorId { get; init; }
}