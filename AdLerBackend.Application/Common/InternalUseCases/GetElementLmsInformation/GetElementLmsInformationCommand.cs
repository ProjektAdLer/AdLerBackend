using AdLerBackend.Application.Common.Responses.Course;

namespace AdLerBackend.Application.Common.InternalUseCases.GetElementLmsInformation;

public record GetElementLmsInformationCommand : CommandWithToken<GetElementLmsInformationResponse>
{
    public int ElementId { get; init; }
    public int WorldId { get; init; }
}