using AdLerBackend.Application.Common.Interfaces;
using AdLerBackend.Application.Common.Responses.World;
using MediatR;

namespace AdLerBackend.Application.World.GetWorldsForAuthor;

public class GetWorldsForAuthorUseCase(IWorldRepository worldRepository, IMediator mediator)
    : IRequestHandler<GetWorldsForAuthorCommand, GetWorldOverviewResponse>
{
    private readonly IMediator _mediator = mediator;

    public async Task<GetWorldOverviewResponse> Handle(GetWorldsForAuthorCommand request,
        CancellationToken cancellationToken)
    {
        var courses = await worldRepository.GetAllForAuthor(request.AuthorId);

        return new GetWorldOverviewResponse
        {
            Worlds = courses.Select(c => new WorldResponse
            {
                WorldId = (int) c.Id!,
                WorldName = c.Name
            }).ToList()
        };
    }
}