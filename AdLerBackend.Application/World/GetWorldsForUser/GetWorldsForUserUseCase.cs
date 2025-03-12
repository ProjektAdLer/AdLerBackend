using AdLerBackend.Application.Common.Interfaces;
using AdLerBackend.Application.Common.Responses.World;
using MediatR;

namespace AdLerBackend.Application.World.GetWorldsForUser;

public class GetWorldsForUserUseCase(ILMS ilms, IWorldRepository worldRepository)
    : IRequestHandler<GetWorldsForUserCommand, GetWorldOverviewResponse>
{
    public async Task<GetWorldOverviewResponse> Handle(GetWorldsForUserCommand request,
        CancellationToken cancellationToken)
    {
        var coursesFromApi = await ilms.GetWorldsForUserAsync(request.WebServiceToken);

        var lmsCoursesIds = coursesFromApi.Courses.Select(c => c.Id).ToList();

        // Get all courses from the database                                                                                                     
        var coursesFromDb = await worldRepository.GetAllAsync();

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