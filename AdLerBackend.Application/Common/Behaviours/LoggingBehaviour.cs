using System.Diagnostics;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AdLerBackend.Application.Common.Behaviours;

public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
{
    private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;

    public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var requestName = request.GetType().Name;

        var requestNameWithGuid = $"{requestName}";

        _logger.LogTrace("[START] {RequestNameWithGuid}", requestNameWithGuid);

        TResponse response;
        var stopwatch = Stopwatch.StartNew();

        try
        {
            response = await next();
        }
        finally
        {
            stopwatch.Stop();
            _logger.LogTrace(
                "[END] {RequestNameWithGuid}; Execution time={StopwatchElapsedMilliseconds}ms",
                requestNameWithGuid, stopwatch.ElapsedMilliseconds);
        }

        return response;
    }
}