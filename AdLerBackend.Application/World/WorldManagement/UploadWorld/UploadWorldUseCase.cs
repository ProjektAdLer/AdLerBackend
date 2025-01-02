using System.ComponentModel.DataAnnotations;
using AdLerBackend.Application.Common.DTOs.Storage;
using AdLerBackend.Application.Common.Interfaces;
using AdLerBackend.Application.Common.Responses.LMSAdapter;
using AdLerBackend.Application.Common.Responses.World;
using AdLerBackend.Application.Configuration;
using AdLerBackend.Application.LMS.GetUserData;
using AdLerBackend.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AdLerBackend.Application.World.WorldManagement.UploadWorld;

public class UploadWorldUseCase(
    ILmsBackupProcessor lmsBackupProcessor,
    IMediator mediator,
    IFileAccess fileAccess,
    IWorldRepository worldRepository,
    ISerialization serialization,
    ILMS lms,
    IOptions<BackendConfig> configuration,
    ILogger<UploadWorldUseCase> logger)
    : IRequestHandler<UploadWorldCommand, CreateWorldResponse>
{
    private readonly BackendConfig _configuration = configuration.Value;

    public async Task<CreateWorldResponse> Handle(UploadWorldCommand request, CancellationToken cancellationToken)
    {
        LMSUserDataResponse userInformation = null;
        WorldAtfResponse courseInformation = null;
        LMSCourseCreationResponse lmsCourseCreationResponse = null;
        Dictionary<string, string> h5PNamesWithPaths = null;
        WorldEntity createdEntity = null;

        try
        {
            userInformation = await GetUserDataFromLms(request, cancellationToken);

            courseInformation = lmsBackupProcessor.GetWorldDescriptionFromBackup(request.ATFFileStream);
            ValidateCourseInformation(courseInformation);

            lmsCourseCreationResponse =
                await lms.UploadCourseWorldToLMS(request.WebServiceToken, request.BackupFileStream);

            h5PNamesWithPaths = StoreH5PFiles(courseInformation, lmsCourseCreationResponse.CourseLmsId,
                request.BackupFileStream);

            var h5PLocationEntities = CreateH5PLocationEntities(courseInformation, h5PNamesWithPaths);
            var courseInformationJsonString = serialization.ClassToJsonString(courseInformation);

            var courseEntity = new WorldEntity(
                lmsCourseCreationResponse.CourseLmsName,
                h5PLocationEntities,
                userInformation.UserId,
                courseInformationJsonString,
                lmsCourseCreationResponse.CourseLmsId
            );

            createdEntity = await worldRepository.AddAsync(courseEntity);

            return CreateSuccessResponse(createdEntity, lmsCourseCreationResponse.CourseLmsId);
        }
        catch (Exception ex)
        {
            await CleanupOnFailure(lmsCourseCreationResponse?.CourseLmsId, createdEntity?.Id,
                request);
            throw;
        }
    }

    private void ValidateCourseInformation(WorldAtfResponse courseInformation)
    {
        var errorList = new List<ValidationResult>();
        var context = new ValidationContext(courseInformation);
        var isValid = Validator.TryValidateObject(courseInformation, context, errorList, true);

        if (!isValid)
        {
            var errorString = errorList.Aggregate("", (current, error) => current + error.ErrorMessage);
            throw new ValidationException(errorString);
        }
    }

    private List<H5PLocationEntity> CreateH5PLocationEntities(WorldAtfResponse courseInformation,
        Dictionary<string, string> h5PNamesWithPaths)
    {
        return (from h5PWithPath in h5PNamesWithPaths
                let h5PName = Guid.Parse(h5PWithPath.Key)
                let h5PInAtf = courseInformation.World.Elements.FirstOrDefault(x => x.ElementUuid == h5PName)
                select new H5PLocationEntity(h5PWithPath.Value, h5PInAtf.ElementId))
            .ToList();
    }

    private CreateWorldResponse CreateSuccessResponse(WorldEntity entity, int lmsCourseId)
    {
        return new CreateWorldResponse
        {
            WorldId = entity.Id!.Value,
            World3DUrl = _configuration.AdLerEngineUrl ?? "Adler Engine URL not set",
            WorldLmsUrl = _configuration.MoodleUrl + "/course/view.php?id=" + lmsCourseId,
            WorldNameInLms = entity.Name
        };
    }

    private async Task CleanupOnFailure(int? lmsWorldId, int? createdWorldEntityId,
        UploadWorldCommand request)
    {
        try
        {
            // H5P Files are stored under the Course files on the file system
            if (lmsWorldId.HasValue)
                fileAccess.DeleteWorld(new WorldDeleteDto
                {
                    WorldInstanceId = lmsWorldId.Value
                });

            if (createdWorldEntityId.HasValue)
                await worldRepository.DeleteAsync(createdWorldEntityId.Value);


            if (lmsWorldId.HasValue) await lms.DeleteCourseAsync(request.WebServiceToken, lmsWorldId.Value);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error while cleaning up after failed World Upload: {Message}", e.Message);
        }
    }

    private async Task<LMSCourseCreationResponse> UploadWorld(UploadWorldCommand request)
    {
        LMSCourseCreationResponse lmsCourseCreationResponse;
        try
        {
            lmsCourseCreationResponse =
                await lms.UploadCourseWorldToLMS(request.WebServiceToken, request.BackupFileStream);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error while uploading World to LMS: {Message}", e.Message);
            throw;
        }

        return lmsCourseCreationResponse;
    }

    private Dictionary<string, string> CreateH5PFilesForWorld(UploadWorldCommand request,
        WorldAtfResponse courseInformation,
        LMSCourseCreationResponse lmsCourseCreationResponse)
    {
        Dictionary<string, string> h5PNamesWithPaths;

        try
        {
            h5PNamesWithPaths = StoreH5PFiles(courseInformation, lmsCourseCreationResponse.CourseLmsId,
                request.BackupFileStream);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error while storing H5P Files: {Message}", e.Message);
            logger.LogInformation("Deleting incomplete World files from File System");
            fileAccess.DeleteWorld(new WorldDeleteDto
            {
                WorldInstanceId = lmsCourseCreationResponse.CourseLmsId
            });
            logger.LogInformation("Deleting Course from LMS");
            lms.DeleteCourseAsync(request.WebServiceToken, lmsCourseCreationResponse.CourseLmsId);
            throw;
        }

        return h5PNamesWithPaths;
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
        if (courseInformation.World.Elements.All(x =>
                x.ElementCategory != "h5p" && x.ElementCategory != "primitiveH5P"))
            return new Dictionary<string, string>();

        var h5PFilesInBackup = lmsBackupProcessor.GetH5PFilesFromBackup(backupFile);

        return fileAccess.StoreH5PFilesForWorld(new WorldStoreH5PDto
        {
            CourseInstanceId = courseLmsId,
            H5PFiles = h5PFilesInBackup
        })!;
    }
}