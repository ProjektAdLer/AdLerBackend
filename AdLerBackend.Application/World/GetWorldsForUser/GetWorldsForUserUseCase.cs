using AdLerBackend.Application.Common.Interfaces;
using AdLerBackend.Application.Common.Responses.World;
using MediatR;

namespace AdLerBackend.Application.World.GetWorldsForUser;

public class GetWorldsForUserUseCase : IRequestHandler<GetWorldsForUserCommand, GetWorldOverviewResponse>
{
    private readonly ILMS _ilms;
    private readonly IWorldRepository _worldRepository;

    public GetWorldsForUserUseCase(ILMS ilms, IWorldRepository worldRepository)
    {
        _ilms = ilms;
        _worldRepository = worldRepository;
    }

    public async Task<GetWorldOverviewResponse> Handle(GetWorldsForUserCommand request,
        CancellationToken cancellationToken)
    {
        var coursesFromApi = await _ilms.GetWorldsForUserAsync(request.WebServiceToken);

        var courseStringList = coursesFromApi.Courses.Select(c => c.Fullname).ToList();

        var coursesFromDb =
            await _worldRepository.GetAllByStrings(courseStringList);

        return new GetWorldOverviewResponse
        {
            Worlds = coursesFromDb.Select(c => new WorldResponse
            {
                WorldId = (int) c.Id!,
                WorldName = c.Name
            }).ToList()
        };
    }
}