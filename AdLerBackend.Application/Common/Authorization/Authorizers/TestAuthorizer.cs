using AdLerBackend.Application.Common.Authorization.Requirements;
using AdLerBackend.Application.World.WorldManagement.DeleteWorld;

namespace AdLerBackend.Application.Common.Authorization.Authorizers;

public class TestAuthorizer : AbstractRequestAuthorizer<DeleteWorldCommand>
{
    public override void BuildPolicy(DeleteWorldCommand request)
    {
        UseRequirement(new TestRequirement());
    }
}