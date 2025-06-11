using AdLerBackend.Application.Common;

namespace AdLerBackend.Application.AdminPanel.CreateUsersByCsv;

public record CreateUsersByCsvCommand : CommandWithToken<bool>
{
    public Stream UserFileStream { get; init; }
}