#pragma warning disable CS8618
namespace AdLerBackend.Application.Common.DTOs.Storage;

public class WorldStoreH5PDto : WorldBaseStorageDto
{
    public IList<H5PDto> H5PFiles { get; set; }
    public int CourseInstanceId { get; set; }
}