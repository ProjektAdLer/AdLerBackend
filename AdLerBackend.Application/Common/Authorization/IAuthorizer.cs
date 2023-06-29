namespace AdLerBackend.Application.Common.Authorization;

public interface IAuthorizer<T>
{
    IEnumerable<IAuthorizationRequirement> Requirements { get; }
    void BuildPolicy(T instance);
}