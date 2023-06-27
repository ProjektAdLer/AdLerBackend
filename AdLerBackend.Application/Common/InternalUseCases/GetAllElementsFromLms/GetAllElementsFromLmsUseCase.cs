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
    private readonly IFileAccess _fileAccess;
    private readonly ILMS _lms;
    private readonly ISerialization _serialization;
    private readonly IWorldRepository _worldRepository;

    public GetAllElementsFromLmsUseCase(IWorldRepository worldRepository, IFileAccess fileAccess,
        ISerialization serialization, ILMS lms)
    {
        _worldRepository = worldRepository;
        _fileAccess = fileAccess;
        _serialization = serialization;
        _lms = lms;
    }

    public async Task<GetAllElementsFromLmsWithAdLerIdResponse> Handle(GetAllElementsFromLmsCommand request,
        CancellationToken cancellationToken)
    {
        var worldEntity = await GetWorldEntity(request.WorldId);
        var dslObject = _serialization.GetObjectFromJsonString<WorldAtfResponse>(worldEntity.AtfJson);
        var worldContent = await _lms.GetWorldContentAsync(request.WebServiceToken, worldEntity.LmsWorldId);
        var modulesWithUuid = await GetModulesWithUuid(request.WebServiceToken, worldEntity.LmsWorldId,
            dslObject.World.Elements.Select(x => x.ElementUuid).ToList());
        var modulesWithId = MapModulesWithAdLerId(dslObject, worldContent, modulesWithUuid);

        return new GetAllElementsFromLmsWithAdLerIdResponse
        {
            ModulesWithAdLerId = modulesWithId.ToList(),
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
        List<string> elementUuids)
    {
        return await _lms.GetLmsElementIdsByUuidsAsync(webServiceToken, lmsWorldId, elementUuids);
    }

    private IEnumerable<ModuleWithId> MapModulesWithAdLerId(WorldAtfResponse dslObject,
        IEnumerable<WorldContent> worldContent, IEnumerable<LmsUuidResponse> modulesWithUuid)
    {
        return modulesWithUuid.Select(mu =>
        {
            var adLerId = dslObject.World.Elements.FirstOrDefault(x => x.ElementUuid == mu.Uuid)?.ElementId;
            var module = worldContent.SelectMany(c => c.Modules).FirstOrDefault(m => m.Id == mu.LmsId);

            return new ModuleWithId
            {
                AdLerId = adLerId!.Value,
                LmsModule = module!,
                IsLocked = module == null
            };
        });
    }
}