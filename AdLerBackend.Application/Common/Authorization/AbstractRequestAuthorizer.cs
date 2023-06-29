namespace AdLerBackend.Application.Common.Authorization;

public abstract class AbstractRequestAuthorizer<TRequest> : IAuthorizer<TRequest>
{
    private HashSet<IAuthorizationRequirement> _requirements { get; } = new();

    public IEnumerable<IAuthorizationRequirement> Requirements => _requirements;

    public abstract void BuildPolicy(TRequest request);

    protected void UseRequirement(IAuthorizationRequirement requirement)
    {
        if (requirement == null) return;
        _requirements.Add(requirement);
    }
}