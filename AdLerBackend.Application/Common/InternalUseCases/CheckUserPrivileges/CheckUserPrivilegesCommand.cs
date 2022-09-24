using MediatR;

namespace AdLerBackend.Application.Common.InternalUseCases.CheckUserPrivileges;

public record CheckUserPrivilegesCommand : CommandWithToken<Unit>
{
    public UserRoles Role { get; init; } = UserRoles.Admin;
}

public enum UserRoles
{
    Admin,
    User
}