using System.Diagnostics;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AdLerBackend.Application.Common.Behaviours;

public class LoggingBehavior<TRequest, TResponse>(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var requestName = request.GetType().Name;

        var requestNameWithGuid = $"{requestName}";

        logger.LogTrace("[START] {RequestNameWithGuid}", requestNameWithGuid);

        TResponse response;
        var stopwatch = Stopwatch.StartNew();

        try
        {
            response = await next();
        }
        finally
        {
            stopwatch.Stop();
            logger.LogTrace(
                "[END] {RequestNameWithGuid}; Execution time={StopwatchElapsedMilliseconds}ms",
                requestNameWithGuid, stopwatch.ElapsedMilliseconds);
        }

        return response;
    }
}