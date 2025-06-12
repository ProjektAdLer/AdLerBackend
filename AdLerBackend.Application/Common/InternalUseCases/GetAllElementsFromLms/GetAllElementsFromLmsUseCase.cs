using AdLerBackend.Application.Common.Exceptions;
using AdLerBackend.Application.Common.Interfaces;
using AdLerBackend.Application.Common.Responses.LMSAdapter;
using AdLerBackend.Application.Common.Responses.World;
using AdLerBackend.Domain.Entities;
using MediatR;

namespace AdLerBackend.Application.Common.InternalUseCases.GetAllElementsFromLms;

public class
    GetAllElementsFromLmsUseCase(
        IWorldRepository worldRepository,
        ISerialization serialization,
        ILMS lms)
    : IRequestHandler<GetAllElementsFromLmsCommand,
        GetAllElementsFromLmsWithAdLerIdResponse>
{
    public async Task<GetAllElementsFromLmsWithAdLerIdResponse> Handle(GetAllElementsFromLmsCommand request,
        CancellationToken cancellationToken)
    {
        var worldEntity = await GetWorldEntity(request.WorldId);
        var atfObject = serialization.GetObjectFromJsonString<WorldAtfResponse>(worldEntity.AtfJson);
        var worldContentFromLms = await lms.GetWorldContentAsync(request.WebServiceToken, worldEntity.LmsWorldId);
        var modulesWithUuid = await GetModulesWithUuid(request.WebServiceToken, worldEntity.LmsWorldId,
            atfObject.World.Elements.Select(x => x.ElementUuid).ToList());
        var modulesWithAdLerId = MapModulesWithAdLerId(atfObject, worldContentFromLms, modulesWithUuid);

        return new GetAllElementsFromLmsWithAdLerIdResponse
        {
            ElementAggregations = modulesWithAdLerId.ToList(),
            LmsCourseId = worldEntity.LmsWorldId,
            // Since we got the World from the DB, we can safely assume that the Id is not null
            AdLerWorldId = worldEntity.Id!.Value
        };
    }

    private async Task<WorldEntity> GetWorldEntity(int worldId)
    {
        var worldEntity = await worldRepository.GetAsync(worldId);
        if (worldEntity == null) throw new NotFoundException($"WorldEntity with the Id {worldId} not found");
        return worldEntity;
    }

    private async Task<IEnumerable<LmsUuidResponse>> GetModulesWithUuid(string webServiceToken, int lmsWorldId,
        List<Guid> elementUuids)
    {
        return await lms.GetLmsElementIdsByUuidsViaPluginAsync(webServiceToken, lmsWorldId, elementUuids);
    }

    // Rider Coverage report provides a false positive for this method. So this is excluded in DotSettings
    private IEnumerable<AdLerLmsElementAggregation> MapModulesWithAdLerId(WorldAtfResponse atfObject,
        IEnumerable<LMSWorldContentResponse> worldContent, IEnumerable<LmsUuidResponse> modulesWithUuid)
    {
        return modulesWithUuid.Select(mu =>
        {
            var adlerElement = atfObject.World.Elements.FirstOrDefault(x => x.ElementUuid == mu.Uuid);
            var module = worldContent.SelectMany(c => c.Modules).FirstOrDefault(m => m.Id == mu.LmsId);

            return new AdLerLmsElementAggregation
            {
                LmsModule = module!,
                IsLocked = module == null,
                AdLerElement = adlerElement!
            };
        });
    }
}