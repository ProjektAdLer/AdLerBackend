using AdLerBackend.Application.Common.Responses.Elements;
using MediatR;

namespace AdLerBackend.Application.Common.ElementStrategies.ScoreElementStrategies.MockPrimitiveH5PStrategy;

public record MockPrimitiveH5PStrategyCommand : CommandWithToken<ScoreElementResponse>
{
    public int ElementId { get; set; }
}

public class
    MockPrimitiveH5PStrategyCommandHandler : IRequestHandler<MockPrimitiveH5PStrategyCommand, ScoreElementResponse>
{
    public Task<ScoreElementResponse> Handle(MockPrimitiveH5PStrategyCommand request,
        CancellationToken cancellationToken)
    {
        return Task.FromResult(new ScoreElementResponse
        {
            IsSuccess = true
        });
    }
}