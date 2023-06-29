namespace AdLerBackend.Application.Common.Authorization.Requirements;

public class TestRequirement : IAuthorizationRequirement
{
    private class TestRequirementHandler : IAuthorizationHandler<TestRequirement>
    {
        public Task<AuthorizationResult> Handle(TestRequirement request, CancellationToken cancellationToken)
        {
            return Task.FromResult(AuthorizationResult.Fail("Das ist MEIN test"));
        }
    }
}