using AdLerBackend.Application.Common.DTOs;
using AdLerBackend.Application.Common.Responses.Course;

namespace AdLerBackend.Application.Common.Interfaces;

public interface ILmsBackupProcessor
{
    public IList<H5PDto> GetH5PFilesFromBackup(Stream backupFile);
    public WorldDtoResponse GetWorldDescriptionFromBackup(Stream dslStream);
}