using System.Diagnostics;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace AdLerBackend.Application.Common.Behaviours;

public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;

    public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger,
        IHttpContextAccessor httpContextAccessor)
    {
        _logger = logger;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var requestName = request.GetType().Name;
        var requestGuid = Guid.NewGuid().ToString();

        var currentRequestTraceIdentifier = _httpContextAccessor.HttpContext?.TraceIdentifier;

        var requestNameWithGuid = $"{requestName} [{requestGuid}]";

        _logger.LogTrace("[START] {RequestNameWithGuid} for httpRequest {CurrentRequestTraceIdentifier}",
            requestNameWithGuid, currentRequestTraceIdentifier);
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
                "[END] {RequestNameWithGuid} for httpRequest {CurrentRequestTraceIdentifier}; Execution time={StopwatchElapsedMilliseconds}ms",
                requestNameWithGuid, currentRequestTraceIdentifier, stopwatch.ElapsedMilliseconds);
        }

        return response;
    }
}