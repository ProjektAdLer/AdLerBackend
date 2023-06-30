using AdLerBackend.Application.Common.Interfaces;
using AdLerBackend.Application.Common.Responses.World;
using MediatR;

namespace AdLerBackend.Application.World.GetWorldsForAuthor;

public class GetWorldsForAuthorUseCase : IRequestHandler<GetWorldsForAuthorCommand, GetWorldOverviewResponse>
{
    private readonly IMediator _mediator;
    private readonly IWorldRepository _worldRepository;

    public GetWorldsForAuthorUseCase(IWorldRepository worldRepository, IMediator mediator)
    {
        _worldRepository = worldRepository;
        _mediator = mediator;
    }

    public async Task<GetWorldOverviewResponse> Handle(GetWorldsForAuthorCommand request,
        CancellationToken cancellationToken)
    {
        var courses = await _worldRepository.GetAllForAuthor(request.AuthorId);

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