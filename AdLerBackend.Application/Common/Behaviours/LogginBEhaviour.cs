using System.Diagnostics;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace AdLerBackend.Application.Common.Behaviours;

public class LogExecutionBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly JsonSerializerSettings _jsonSerializerSettings;
    private readonly ILogger<TRequest> _logger;

    public LogExecutionBehavior(
        ILogger<TRequest> logger,
        JsonSerializerSettings jsonSerializerSettings = null)
    {
        _logger = logger;
        _jsonSerializerSettings = jsonSerializerSettings ?? new JsonSerializerSettings();
    }

    public async Task<TResponse> Handle(
        TRequest request,
        CancellationToken cancellationToken,
        RequestHandlerDelegate<TResponse> next)
    {
        var guid = Guid.NewGuid();
        var timer = new Stopwatch();
        TResponse response1;
        using (_logger.BeginScope("{MeditatorRequestName} with {MeditatorRequestData}, correlation id {CorrelationId}",
                   typeof(TRequest).Name, JsonConvert.SerializeObject(request, _jsonSerializerSettings), guid))
        {
            try
            {
                _logger.LogDebug("Handler for {MeditatorRequestName} starting", typeof(TRequest).Name);
                timer.Start();
                var response2 = await next();
                timer.Stop();
                _logger.LogDebug("Handler for {MeditatorRequestName} finished in {ElapsedMilliseconds}ms",
                    typeof(TRequest).Name, timer.Elapsed.TotalMilliseconds);
                response1 = response2;
            }
            catch (Exception ex)
            {
                timer.Stop();
                _logger.LogError(ex,
                    "Handler for {MeditatorRequestName} failed in {ElapsedMilliseconds}ms", typeof(TRequest).Name,
                    timer.Elapsed.TotalMilliseconds);
                throw;
            }
        }

        return response1;
    }
}