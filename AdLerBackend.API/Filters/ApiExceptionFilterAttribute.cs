using System.Net;
using AdLerBackend.API.Common;
using AdLerBackend.Application.Common.Exceptions;
using AdLerBackend.Application.Common.Exceptions.LMSAdapter;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AdLerBackend.API.Filters;

/// <summary>
///     Handles exceptions thrown in the API
/// </summary>
public class ApiExceptionFilterAttribute : ExceptionFilterAttribute
{
    private readonly IDictionary<Type, Action<ExceptionContext>> _exceptionHandlers;
    private readonly ILogger<ApiExceptionFilterAttribute> _logger;

    /// <summary>
    /// </summary>
    public ApiExceptionFilterAttribute(ILogger<ApiExceptionFilterAttribute> logger)
    {
        _logger = logger;
        _exceptionHandlers = new Dictionary<Type, Action<ExceptionContext>>
        {
            {typeof(ValidationException), HandleValidationException},
            {typeof(InvalidLmsLoginException), HandleLmsLoginException},
            {typeof(InvalidTokenException), HandleInvalidTokenException},
            {typeof(NotFoundException), HandleNotFoundException},
            {typeof(ForbiddenAccessException), HandleForbiddenAccessException},
            {typeof(WorldCreationException), HandleWorldCreationException},
            {typeof(LmsException), HandleGenericLmsException}
        };
    }

    /// <summary>
    ///     Handles the exception
    /// </summary>
    /// <param name="context"></param>
    public override void OnException(ExceptionContext context)
    {
        HandleException(context);
        base.OnException(context);
    }

    private void HandleException(ExceptionContext context)
    {
        var type = context.Exception.GetType();
        if (_exceptionHandlers.TryGetValue(type, out var handler))
            handler.Invoke(context);
        else
            HandleUnknownException(context);
    }

    private void SetResult(ExceptionContext context, ProblemDetails problemDetails)
    {
        context.Result = new ObjectResult(problemDetails);
        context.ExceptionHandled = true;
    }

    private void HandleGenericLmsException(ExceptionContext context)
    {
        var exception = (LmsException) context.Exception;
        var problemDetails = new ProblemDetails
        {
            Detail = exception.Message,
            Status = StatusCodes.Status500InternalServerError,
            Type = ErrorCodes.LmsError
        };

        _logger.LogError("The LMS adapter encountered an error {Message}", exception.Message);

        SetResult(context, problemDetails);
    }

    private void HandleWorldCreationException(ExceptionContext context)
    {
        var exception = (WorldCreationException) context.Exception;

        var problemDetails = new ProblemDetails
        {
            Detail = exception.Message,
            Title = "World creation failed",
            Status = StatusCodes.Status409Conflict,
            Type = ErrorCodes.WorldCreationErrorDuplicate
        };

        _logger.LogInformation("World creation failed", exception.Message);

        SetResult(context, problemDetails);
    }


    private void HandleForbiddenAccessException(ExceptionContext context)
    {
        var exception = (ForbiddenAccessException) context.Exception;
        var problemDetails = new ProblemDetails
        {
            Detail = exception.Message,
            Title = "Forbidden Access",
            Status = StatusCodes.Status403Forbidden,
            Type = ErrorCodes.Forbidden
        };

        _logger.LogInformation("Forbidden Access", exception.Message);

        SetResult(context, problemDetails);
    }

    private void HandleNotFoundException(ExceptionContext context)
    {
        var exception = (NotFoundException) context.Exception;
        var problemDetails = new ProblemDetails
        {
            Title = "The requested Resource was not found",
            Detail = exception.Message,
            Status = StatusCodes.Status404NotFound,
            Type = ErrorCodes.NotFound
        };

        _logger.LogInformation("Resource not found", exception.Message);

        SetResult(context, problemDetails);
    }

    private void HandleInvalidTokenException(ExceptionContext context)
    {
        var problemDetails = new ProblemDetails
        {
            Title = "Invalid token",
            Status = (int) HttpStatusCode.Unauthorized,
            Type = ErrorCodes.LmsTokenInvalid,
            Detail = "The provided token is invalid"
        };

        _logger.LogInformation("Invalid token provided");

        SetResult(context, problemDetails);
    }

    private void HandleUnknownException(ExceptionContext context)
    {
        var problemDetails = new ProblemDetails
        {
            Title = "An unknown error occurred while processing your request",
            Status = StatusCodes.Status500InternalServerError,
            Detail = context.Exception.Message,
            Type = ErrorCodes.UnknownError
        };

        _logger.LogError(context.Exception, "An unknown error occurred while processing the request");

        SetResult(context, problemDetails);
    }

    private void HandleValidationException(ExceptionContext context)
    {
        var exception = (ValidationException) context.Exception;

        var problemDetails = new ValidationProblemDetails(exception.Errors)
        {
            Type = ErrorCodes.ValidationError,
            Status = StatusCodes.Status400BadRequest,
            Title = "Validation Error"
        };

        _logger.LogInformation("Validation Error: {message}", exception.Message);

        SetResult(context, problemDetails);
    }

    private void HandleLmsLoginException(ExceptionContext context)
    {
        var problemDetails = new ProblemDetails
        {
            Title = "LMS login error",
            Status = StatusCodes.Status401Unauthorized,
            Type = ErrorCodes.InvalidLogin,
            Detail = "The Lms Login Data Provided is wrong"
        };

        _logger.LogInformation("User entered the wrong LMS login data");

        SetResult(context, problemDetails);
    }
}