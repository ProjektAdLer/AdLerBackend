#pragma warning disable CS8618
namespace AdLerBackend.Application.Common.DTOs.Storage;

public class StoreWorldAtfDto : WorldBaseStorageDto
{
    public Stream AtfFile { get; set; }
}