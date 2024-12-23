using System.ComponentModel.DataAnnotations;
using AdLerBackend.Application.Common.DTOs.Storage;
using AdLerBackend.Application.Common.Interfaces;
using AdLerBackend.Application.Common.Responses.LMSAdapter;
using AdLerBackend.Application.Common.Responses.World;
using AdLerBackend.Application.Configuration;
using AdLerBackend.Application.LMS.GetUserData;
using AdLerBackend.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Options;

namespace AdLerBackend.Application.World.WorldManagement.UploadWorld;

public class UploadWorldUseCase(
    ILmsBackupProcessor lmsBackupProcessor,
    IMediator mediator,
    IFileAccess fileAccess,
    IWorldRepository worldRepository,
    ISerialization serialization,
    ILMS lms,
    IOptions<BackendConfig> configuration)
    : IRequestHandler<UploadWorldCommand, CreateWorldResponse>
{
    private readonly BackendConfig _configuration = configuration.Value;

    public async Task<CreateWorldResponse> Handle(UploadWorldCommand request, CancellationToken cancellationToken)
    {
        var userInformation = await GetUserDataFromLms(request, cancellationToken);

        var courseInformation = lmsBackupProcessor.GetWorldDescriptionFromBackup(request.ATFFileStream);


        var errorList = new List<ValidationResult>();
        var context = new ValidationContext(courseInformation);
        var isValid = Validator.TryValidateObject(courseInformation, context, errorList, true);

        if (!isValid)
        {
            var errorString = errorList.Aggregate("", (current, error) => current + error.ErrorMessage);
            throw new ValidationException(errorString);
        }


        var lmsCourseCreationResponse =
            await lms.UploadCourseWorldToLMS(request.WebServiceToken, request.BackupFileStream);

        var h5PNamesWithPaths = StoreH5PFiles(courseInformation, lmsCourseCreationResponse.CourseLmsId,
            request.BackupFileStream);

        var h5PLocationEntities = (from h5PWithPath in h5PNamesWithPaths
                let h5PName = Guid.Parse(h5PWithPath.Key)
                let h5PInAtf = courseInformation.World.Elements.FirstOrDefault(x => x.ElementUuid == h5PName)
                select new H5PLocationEntity(h5PWithPath.Value, h5PInAtf.ElementId))
            .ToList();


        var courseInformationJsonString = serialization.ClassToJsonString(courseInformation);

        var courseEntity = new WorldEntity(
            lmsCourseCreationResponse.CourseLmsName,
            h5PLocationEntities,
            userInformation.UserId,
            courseInformationJsonString,
            lmsCourseCreationResponse.CourseLmsId
        );

        var createdEntity = await worldRepository.AddAsync(courseEntity);

        return new CreateWorldResponse
        {
            // When the world is created, the id is set
            WorldId = createdEntity.Id!.Value,
            World3DUrl = _configuration.AdLerEngineUrl ?? "Adler Engine URL not set",
            WorldLmsUrl = _configuration.MoodleUrl + "/course/view.php?id=" + lmsCourseCreationResponse.CourseLmsId,
            WorldNameInLms = courseEntity.Name
        };
    }

    private async Task<LMSUserDataResponse> GetUserDataFromLms(UploadWorldCommand request,
        CancellationToken cancellationToken)
    {
        var userInformation = await mediator.Send(new GetLMSUserDataCommand
        {
            WebServiceToken = request.WebServiceToken
        }, cancellationToken);
        return userInformation;
    }


    private Dictionary<string, string> StoreH5PFiles(WorldAtfResponse courseInformation, int courseLmsId,
        Stream backupFile)
    {
        if (courseInformation.World.Elements.All(x => x.ElementCategory != "h5p"))
            return new Dictionary<string, string>();

        var h5PFilesInBackup = lmsBackupProcessor.GetH5PFilesFromBackup(backupFile);

        return fileAccess.StoreH5PFilesForWorld(new WorldStoreH5PDto
        {
            CourseInstanceId = courseLmsId,
            H5PFiles = h5PFilesInBackup
        })!;
    }
}