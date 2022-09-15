using AdLerBackend.Application.Common.DTOs;
using AdLerBackend.Application.Common.Responses.Course;

namespace AdLerBackend.Application.Common.Interfaces;

public interface ILmsBackupProcessor
{
    public IList<H5PDto> GetH5PFilesFromBackup(Stream backupFile);
    public LearningWorldDtoResponse GetLevelDescriptionFromBackup(Stream dslStream);
}