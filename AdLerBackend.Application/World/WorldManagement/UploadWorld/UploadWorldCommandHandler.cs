using AdLerBackend.Application.Common.DTOs.Storage;
using AdLerBackend.Application.Common.Exceptions;
using AdLerBackend.Application.Common.Interfaces;
using AdLerBackend.Application.Common.InternalUseCases.CheckUserPrivileges;
using AdLerBackend.Application.Common.Responses.Course;
using AdLerBackend.Application.Common.Responses.LMSAdapter;
using AdLerBackend.Application.LMS.GetUserData;
using AdLerBackend.Application.World.ValidateATFFile;
using AdLerBackend.Domain.Entities;
using MediatR;

namespace AdLerBackend.Application.World.WorldManagement.UploadWorld;

public class UploadWorldCommandHandler : IRequestHandler<UploadWorldCommand, bool>
{
    private readonly IFileAccess _fileAccess;
    private readonly ILMS _lms;
    private readonly ILmsBackupProcessor _lmsBackupProcessor;
    private readonly IMediator _mediator;
    private readonly ISerialization _serialization;
    private readonly IWorldRepository _worldRepository;

    public UploadWorldCommandHandler(ILmsBackupProcessor lmsBackupProcessor, IMediator mediator,
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
        // Validate ATF File
        await _mediator.Send(new ValidateATFFileCommand
        {
            ATFFileStream = request.ATFFileStream
        }, cancellationToken);

        // check if user is Admin
        await _mediator.Send(new CheckUserPrivilegesCommand
        {
            WebServiceToken = request.WebServiceToken
        }, cancellationToken);

        var userInformation = await _mediator.Send(new GetLMSUserDataCommand
        {
            WebServiceToken = request.WebServiceToken
        }, cancellationToken);


        var courseInformation = _lmsBackupProcessor.GetWorldDescriptionFromBackup(request.ATFFileStream);


        var existsCourseForAuthor = await _worldRepository.ExistsForAuthor(userInformation.UserId,
            courseInformation.World.LmsElementIdentifier.Value);

        if (existsCourseForAuthor) throw new WorldCreationException("World already exists in Database");

        // Upload the Backup File to the LMS
        // disabled until LMS is ready - PG
        //await _lms.UploadCourseWorldToLMS(request.WebServiceToken, request.BackupFileStream);

        var atfLocation = _fileAccess.StoreATFFileForWorld(new StoreWorldATFDto
        {
            AuthorId = userInformation.UserId,
            ATFFile = request.ATFFileStream,
            WorldInforamtion = courseInformation
        });


        // Get Course DSL 
        await using var fileStream = _fileAccess.GetReadFileStream(atfLocation);

        // Parse DSL File
        var dslFile = await _serialization.GetObjectFromJsonStreamAsync<WorldDtoResponse>(fileStream);

        var storedH5PFilePaths = StoreH5PFiles(courseInformation, userInformation, request.BackupFileStream);

        var h5PLocationEntities = (from filePath in storedH5PFilePaths
            let fileName = Path.GetFileName(filePath)
            let element = dslFile.World.Elements.FirstOrDefault(x => x.LmsElementIdentifier?.Value == fileName)
            where element != null
            select new H5PLocationEntity(filePath, element.ElementId)).ToList();

        var courseEntity = new WorldEntity(
            courseInformation.World.LmsElementIdentifier.Value,
            h5PLocationEntities,
            atfLocation,
            userInformation.UserId
        );

        await _worldRepository.AddAsync(courseEntity);


        return true;
    }

    private List<string> StoreH5PFiles(WorldDtoResponse courseInformation, LMSUserDataResponse userData,
        Stream backupFile)
    {
        var storedH5PFilePaths = new List<string>();
        if (courseInformation.World.Elements.Any(x => x.ElementCategory == "h5p"))
        {
            var h5PFilesInBackup = _lmsBackupProcessor.GetH5PFilesFromBackup(backupFile);
            storedH5PFilePaths = _fileAccess.StoreH5PFilesForWorld(new WorldStoreH5PDto
            {
                AuthorId = userData.UserId,
                WorldInforamtion = courseInformation,
                H5PFiles = h5PFilesInBackup
            });
        }

        return storedH5PFilePaths!;
    }
}