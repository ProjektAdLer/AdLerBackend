using AdLerBackend.Application.Common.DTOs.Storage;
using AdLerBackend.Application.Common.Interfaces;
using AdLerBackend.Application.Common.Responses.LMSAdapter;
using AdLerBackend.Application.Common.Responses.World;
using AdLerBackend.Application.Configuration;
using AdLerBackend.Application.LMS.GetUserData;
using AdLerBackend.Application.World.ValidateATFFile;
using AdLerBackend.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Options;

namespace AdLerBackend.Application.World.WorldManagement.UploadWorld;

public class UploadWorldUseCase : IRequestHandler<UploadWorldCommand, CreateWorldResponse>
{
    private readonly BackendConfig _configuration;
    private readonly IFileAccess _fileAccess;
    private readonly ILMS _lms;
    private readonly ILmsBackupProcessor _lmsBackupProcessor;
    private readonly IMediator _mediator;
    private readonly ISerialization _serialization;
    private readonly IWorldRepository _worldRepository;

    public UploadWorldUseCase(ILmsBackupProcessor lmsBackupProcessor, IMediator mediator,
        IFileAccess fileAccess, IWorldRepository worldRepository, ISerialization serialization, ILMS lms,
        IOptions<BackendConfig> configuration)
    {
        _lmsBackupProcessor = lmsBackupProcessor;
        _mediator = mediator;
        _fileAccess = fileAccess;
        _worldRepository = worldRepository;
        _serialization = serialization;
        _lms = lms;

        _configuration = configuration.Value;
    }

    public async Task<CreateWorldResponse> Handle(UploadWorldCommand request, CancellationToken cancellationToken)
    {
        // TODO: Skip Validation for development purposes
        //await ValidateAtfFile(request, cancellationToken);

        var userInformation = await GetUserDataFromLms(request, cancellationToken);

        var courseInformation = _lmsBackupProcessor.GetWorldDescriptionFromBackup(request.ATFFileStream);

        var lmsCourseCreationResponse =
            await _lms.UploadCourseWorldToLMS(request.WebServiceToken, request.BackupFileStream);

        var h5PNamesWithPaths = StoreH5PFiles(courseInformation, lmsCourseCreationResponse.CourseLmsId,
            request.BackupFileStream);

        var h5PLocationEntities = (from h5PWithPath in h5PNamesWithPaths
                let h5PName = h5PWithPath.Key
                let h5PInAtf = courseInformation.World.Elements.FirstOrDefault(x => x.ElementUuid == h5PName)
                select new H5PLocationEntity(h5PWithPath.Value, h5PInAtf.ElementId))
            .ToList();


        var courseInformationJsonString = _serialization.ClassToJsonString(courseInformation);

        var courseEntity = new WorldEntity(
            lmsCourseCreationResponse.CourseLmsName,
            h5PLocationEntities,
            userInformation.UserId,
            courseInformationJsonString,
            lmsCourseCreationResponse.CourseLmsId
        );

        await _worldRepository.AddAsync(courseEntity);

        return new CreateWorldResponse
        {
            World3DUrl = _configuration.AdLerEngineUrl ?? "Adler Engine URL not set",
            WorldLmsUrl = _configuration.MoodleUrl + "/course/view.php?id=" + lmsCourseCreationResponse.CourseLmsId,
            WorldNameInLms = courseEntity.Name
        };
    }

    private async Task<LMSUserDataResponse> GetUserDataFromLms(UploadWorldCommand request,
        CancellationToken cancellationToken)
    {
        var userInformation = await _mediator.Send(new GetLMSUserDataCommand
        {
            WebServiceToken = request.WebServiceToken
        }, cancellationToken);
        return userInformation;
    }

    private async Task ValidateAtfFile(UploadWorldCommand request, CancellationToken cancellationToken)
    {
        await _mediator.Send(new ValidateATFFileCommand
        {
            ATFFileStream = request.ATFFileStream
        }, cancellationToken);
    }

    private Dictionary<string, string> StoreH5PFiles(WorldAtfResponse courseInformation, int courseLmsId,
        Stream backupFile)
    {
        if (courseInformation.World.Elements.All(x => x.ElementCategory != "h5p"))
            return new Dictionary<string, string>();

        var h5PFilesInBackup = _lmsBackupProcessor.GetH5PFilesFromBackup(backupFile);

        return _fileAccess.StoreH5PFilesForWorld(new WorldStoreH5PDto
        {
            CourseInstanceId = courseLmsId,
            H5PFiles = h5PFilesInBackup
        })!;
    }
}