#pragma warning disable CS8618
namespace AdLerBackend.Application.Common.DTOs.Storage;

public class StoreWorldATFDto : WorldBaseStorageDto
{
    public Stream ATFFile { get; set; }
}