using AdLerBackend.Application.Common.Responses.Course;

namespace AdLerBackend.Application.Common.InternalUseCases.GetAllElementsFromLms;

public record GetAllElementsFromLmsCommand : CommandWithToken<GetAllElementsFromLmsResponse>
{
    public int WorldId { get; set; }
}