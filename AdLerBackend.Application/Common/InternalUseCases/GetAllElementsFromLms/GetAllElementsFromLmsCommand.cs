using AdLerBackend.Application.Common.Responses.World;

namespace AdLerBackend.Application.Common.InternalUseCases.GetAllElementsFromLms;

public record GetAllElementsFromLmsCommand : CommandWithToken<GetAllElementsFromLmsWithAdLerIdResponse>
{
    public int WorldId { get; set; }
}