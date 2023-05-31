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

        var lmsCoursesIds = coursesFromApi.Courses.Select(c => c.Id).ToList();

        // Get all courses from the database                                                                                                     
        var coursesFromDb = await _worldRepository.GetAllAsync();

        // Get all courses that are in the Database and in the LMS
        var coursesInDbAndInLms = coursesFromDb.Where(c => lmsCoursesIds.Contains(c.LmsWorldId));

        return new GetWorldOverviewResponse
        {
            Worlds = coursesInDbAndInLms.Select(c => new WorldResponse
            {
                WorldId = (int) c.Id!,
                WorldName = c.Name
            }).ToList() ?? new List<WorldResponse>()
        };
    }
}