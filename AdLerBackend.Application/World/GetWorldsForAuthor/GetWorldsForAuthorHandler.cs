using AdLerBackend.Application.Common.Interfaces;
using AdLerBackend.Application.Common.InternalUseCases.CheckUserPrivileges;
using AdLerBackend.Application.Common.Responses.Course;
using MediatR;

namespace AdLerBackend.Application.World.GetWorldsForAuthor;

public class GetWorldsForAuthorHandler : IRequestHandler<GetWorldsForAuthorCommand, GetWorldOverviewResponse>
{
    private readonly IMediator _mediator;
    private readonly IWorldRepository _worldRepository;

    public GetWorldsForAuthorHandler(IWorldRepository worldRepository, IMediator mediator)
    {
        _worldRepository = worldRepository;
        _mediator = mediator;
    }

    public async Task<GetWorldOverviewResponse> Handle(GetWorldsForAuthorCommand request,
        CancellationToken cancellationToken)
    {
        // check if user is Admin
        await _mediator.Send(new CheckUserPrivilegesCommand
        {
            WebServiceToken = request.WebServiceToken
        }, cancellationToken);

        var courses = await _worldRepository.GetAllForAuthor(request.AuthorId);

        return new GetWorldOverviewResponse
        {
            Worlds = courses.Select(c => new WorldResponse
            {
                WorldId = c.Id,
                WorldName = c.Name
            }).ToList()
        };
    }
}