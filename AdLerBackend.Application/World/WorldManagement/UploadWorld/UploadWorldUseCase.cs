using AdLerBackend.Application.Common.DTOs.Storage;
using AdLerBackend.Application.Common.Exceptions;
using AdLerBackend.Application.Common.Interfaces;
using AdLerBackend.Application.Common.InternalUseCases.CheckUserPrivileges;
using AdLerBackend.Application.Common.Responses.LMSAdapter;
using AdLerBackend.Application.Common.Responses.World;
using AdLerBackend.Application.LMS.GetUserData;
using AdLerBackend.Application.World.ValidateATFFile;
using AdLerBackend.Domain.Entities;
using MediatR;

namespace AdLerBackend.Application.World.WorldManagement.UploadWorld;

public class UploadWorldUseCase : IRequestHandler<UploadWorldCommand, bool>
{
    private readonly IFileAccess _fileAccess;
    private readonly ILMS _lms;
    private readonly ILmsBackupProcessor _lmsBackupProcessor;
    private readonly IMediator _mediator;
    private readonly ISerialization _serialization;
    private readonly IWorldRepository _worldRepository;

    public UploadWorldUseCase(ILmsBackupProcessor lmsBackupProcessor, IMediator mediator,
        IFileAccess fileAccess, IWorldRepository worldRepository, ISerialization serialization, ILMS lms)
    {
        _lmsBackupProcessor = lmsBackupProcessor;
        _mediator = mediator;
        _fileAccess = fileAccess;
        _worldRepository = worldRepository;
        _serialization = serialization;
        _lms = lms;
    }

    public async Task<bool> Handle(UploadWorldCommand request, CancellationToken cancellationToken)
    {
        await ValidateAtfFile(request, cancellationToken);

        await ThrowIfUserIsNotAdmin(request, cancellationToken);

        var userInformation = await GetUserDataFromLms(request, cancellationToken);


        var courseInformation = _lmsBackupProcessor.GetWorldDescriptionFromBackup(request.ATFFileStream);


        var existsCourseForAuthor = await _worldRepository.ExistsForAuthor(userInformation.UserId,
            courseInformation.World.WorldName);

        if (existsCourseForAuthor) throw new WorldCreationException("World already exists in Database");

        // Upload the Backup File to the LMS
        // disabled until LMS is ready - PG
        //await _lms.UploadCourseWorldToLMS(request.WebServiceToken, request.BackupFileStream);

        var atfLocation = _fileAccess.StoreAtfFileForWorld(new StoreWorldAtfDto
        {
            AuthorId = userInformation.UserId,
            AtfFile = request.ATFFileStream,
            WorldInformation = courseInformation
        });


        // Get Course DSL 
        await using var fileStream = _fileAccess.GetReadFileStream(atfLocation);

        // Parse DSL File
        var dslFile = await _serialization.GetObjectFromJsonStreamAsync<WorldAtfResponse>(fileStream);

        var h5PNamesWithPaths = StoreH5PFiles(courseInformation, userInformation, request.BackupFileStream);

        var h5PLocationEntities = (from h5PWithPath in h5PNamesWithPaths
                let h5PName = h5PWithPath.Key
                let h5PInDsl = dslFile.World.Elements.FirstOrDefault(x => x.ElementName == h5PName)
                select new H5PLocationEntity(h5PWithPath.Value, h5PInDsl.ElementId))
            .ToList();

        var courseEntity = new WorldEntity(
            courseInformation.World.WorldName,
            h5PLocationEntities,
            atfLocation,
            userInformation.UserId,
            "UUID"
        );

        await _worldRepository.AddAsync(courseEntity);


        return true;
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

    private async Task ThrowIfUserIsNotAdmin(UploadWorldCommand request, CancellationToken cancellationToken)
    {
        await _mediator.Send(new CheckUserPrivilegesCommand
        {
            WebServiceToken = request.WebServiceToken
        }, cancellationToken);
    }

    private async Task ValidateAtfFile(UploadWorldCommand request, CancellationToken cancellationToken)
    {
        await _mediator.Send(new ValidateATFFileCommand
        {
            ATFFileStream = request.ATFFileStream
        }, cancellationToken);
    }

    private Dictionary<string, string> StoreH5PFiles(WorldAtfResponse courseInformation, LMSUserDataResponse userData,
        Stream backupFile)
    {
        if (courseInformation.World.Elements.All(x => x.ElementCategory != "h5p"))
            return new Dictionary<string, string>();

        var h5PFilesInBackup = _lmsBackupProcessor.GetH5PFilesFromBackup(backupFile);

        return _fileAccess.StoreH5PFilesForWorld(new WorldStoreH5PDto
        {
            AuthorId = userData.UserId,
            WorldInformation = courseInformation,
            H5PFiles = h5PFilesInBackup
        })!;
    }
}