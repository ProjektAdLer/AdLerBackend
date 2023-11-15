#pragma warning disable CS8618
using AdLerBackend.Application.Common;
using AdLerBackend.Application.Common.Responses.World;

namespace AdLerBackend.Application.World.WorldManagement.UploadWorld;

public record UploadWorldCommand : CommandWithToken<CreateWorldResponse>
{
    public Stream BackupFileStream { get; set; }
    public Stream ATFFileStream { get; set; }
}
#pragma warning restore CS8618