#pragma warning disable CS8618
using AdLerBackend.Application.Common;

namespace AdLerBackend.Application.World.WorldManagement.UploadWorld;

public record UploadWorldCommand : CommandWithToken<bool>
{
    public Stream BackupFileStream { get; set; }
    public Stream ATFFileStream { get; set; }
}
#pragma warning restore CS8618