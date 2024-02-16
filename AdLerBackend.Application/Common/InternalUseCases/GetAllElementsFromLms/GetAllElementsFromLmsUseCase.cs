using AdLerBackend.Application.Common.Exceptions;
using AdLerBackend.Application.Common.Interfaces;
using AdLerBackend.Application.Common.Responses.LMSAdapter;
using AdLerBackend.Application.Common.Responses.World;
using AdLerBackend.Domain.Entities;
using MediatR;

namespace AdLerBackend.Application.Common.InternalUseCases.GetAllElementsFromLms;

public class
    GetAllElementsFromLmsUseCase : IRequestHandler<GetAllElementsFromLmsCommand,
    GetAllElementsFromLmsWithAdLerIdResponse>
{
    private readonly ILMS _lms;
    private readonly ISerialization _serialization;
    private readonly IWorldRepository _worldRepository;

    public GetAllElementsFromLmsUseCase(IWorldRepository worldRepository,
        ISerialization serialization, ILMS lms)
    {
        _worldRepository = worldRepository;
        _serialization = serialization;
        _lms = lms;
    }

    public async Task<GetAllElementsFromLmsWithAdLerIdResponse> Handle(GetAllElementsFromLmsCommand request,
        CancellationToken cancellationToken)
    {
        var worldEntity = await GetWorldEntity(request.WorldId);
        var atfObject = _serialization.GetObjectFromJsonString<WorldAtfResponse>(worldEntity.AtfJson);
        var worldContentFromLms = await _lms.GetWorldContentAsync(request.WebServiceToken, worldEntity.LmsWorldId);
        var modulesWithUuid = await GetModulesWithUuid(request.WebServiceToken, worldEntity.LmsWorldId,
            atfObject.World.Elements.Select(x => x.ElementUuid).ToList());
        var modulesWithAdLerId = MapModulesWithAdLerId(atfObject, worldContentFromLms, modulesWithUuid);

        return new GetAllElementsFromLmsWithAdLerIdResponse
        {
            ModulesWithAdLerId = modulesWithAdLerId.ToList(),
            LmsCourseId = worldEntity.LmsWorldId
        };
    }

    private async Task<WorldEntity> GetWorldEntity(int worldId)
    {
        var worldEntity = await _worldRepository.GetAsync(worldId);
        if (worldEntity == null) throw new NotFoundException($"WorldEntity with the Id {worldId} not found");
        return worldEntity;
    }

    private async Task<IEnumerable<LmsUuidResponse>> GetModulesWithUuid(string webServiceToken, int lmsWorldId,
        List<Guid> elementUuids)
    {
        return await _lms.GetLmsElementIdsByUuidsAsync(webServiceToken, lmsWorldId, elementUuids);
    }

    private IEnumerable<ModuleWithId> MapModulesWithAdLerId(WorldAtfResponse atfObject,
        IEnumerable<LMSWorldContentResponse> worldContent, IEnumerable<LmsUuidResponse> modulesWithUuid)
    {
        return modulesWithUuid.Select(mu =>
        {
            var adLerId = atfObject.World.Elements.FirstOrDefault(x => x.ElementUuid == mu.Uuid)?.ElementId;
            var module = worldContent.SelectMany(c => c.Modules).FirstOrDefault(m => m.Id == mu.LmsId);

            return new ModuleWithId
            {
                AdLerId = adLerId!.Value,
                LmsModule = module!,
                IsLocked = module == null,
                LmsModuleUuid = mu.Uuid
            };
        });
    }
}