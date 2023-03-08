using AdLerBackend.API.Common.ProblemDetails;
using AdLerBackend.Application.Common.Exceptions;
using AdLerBackend.Application.Common.Exceptions.LMSAdapter;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AdLerBackend.API.Filters;

public class ApiExceptionFilterAttribute : ExceptionFilterAttribute
{
    private readonly IDictionary<Type, Action<ExceptionContext>> _exceptionHandlers;

    public ApiExceptionFilterAttribute()
    {
        _exceptionHandlers = new Dictionary<Type, Action<ExceptionContext>>
        {
            {typeof(ValidationException), HandleValidationException},
            {typeof(InvalidLMSLoginException), HandleLMSLoginException},
            {typeof(InvalidTokenException), HandleInvalidTokenException},
            {typeof(NotFoundException), HandleNotFoundException},
            {typeof(ForbiddenAccessException), HandleForbiddenAccessException},
            {typeof(WorldCreationException), HandleWorldCreationException},
            {typeof(LmsException), HandleGenericLmsException}
        };
    }

    public override void OnException(ExceptionContext context)
    {
        HandleException(context);
        base.OnException(context);
    }

    private void HandleGenericLmsException(ExceptionContext context)
    {
        var exception = context.Exception as LmsException;
        var problemDetails = new ProblemDetails
        {
            Detail = exception.Message,
            Status = StatusCodes.Status500InternalServerError,
            Title = "The LMS adapter encountered an error"
        };

        context.Result = new ObjectResult(problemDetails);
    }

    private void HandleWorldCreationException(ExceptionContext context)
    {
        var exception = context.Exception as WorldCreationException;
        var problemDateils = new ProblemDetails
        {
            Detail = exception.Message,
            Title = "World creation failed",
            Status = StatusCodes.Status409Conflict,
            Instance = context.HttpContext.Request.Path
        };

        context.Result = new ConflictObjectResult(problemDateils);
    }

    private void HandleForbiddenAccessException(ExceptionContext context)
    {
        var exception = context.Exception as ForbiddenAccessException;
        var problemDetails = new ProblemDetails
        {
            Instance = context.HttpContext.Request.Path,
            Detail = exception!.Message,
            Title = "Forbidden Access"
        };

        context.Result = new UnauthorizedObjectResult(problemDetails)
        {
            StatusCode = StatusCodes.Status403Forbidden
        };
    }

    private void HandleNotFoundException(ExceptionContext context)
    {
        var exception = (NotFoundException) context.Exception;
        var details = new ProblemDetails
        {
            Title = "The requestet Resource was not found",
            Detail = exception.Message,
            Status = StatusCodes.Status404NotFound
        };

        context.Result = new NotFoundObjectResult(details);
        context.ExceptionHandled = true;
    }

    private void HandleInvalidTokenException(ExceptionContext context)
    {
        var details = new MoodleTokenProblemDetails();

        context.Result = new UnauthorizedObjectResult(details);

        context.ExceptionHandled = true;
    }

    private void HandleException(ExceptionContext context)
    {
        var type = context.Exception.GetType();
        if (_exceptionHandlers.ContainsKey(type))
            _exceptionHandlers[type].Invoke(context);
        else
            HandleUnknownException(context);
    }

    private void HandleUnknownException(ExceptionContext context)
    {
        var details = new ProblemDetails
        {
            Title = "An unknown error occurred while processing your request.",
            Status = StatusCodes.Status500InternalServerError,
            Detail = context.Exception.Message
        };

        context.Result = new ObjectResult(details)
        {
            StatusCode = StatusCodes.Status500InternalServerError
        };

        context.ExceptionHandled = true;
    }

    private void HandleValidationException(ExceptionContext context)
    {
        var exception = (ValidationException) context.Exception;

        var details = new ValidationProblemDetails(exception.Errors)
        {
            Type = "Validation Error"
        };

        context.Result = new BadRequestObjectResult(details);

        context.ExceptionHandled = true;
    }

    private void HandleLMSLoginException(ExceptionContext context)
    {
        var details = new MoodleLoginProblemDetails
        {
            Detail = "The Moodle Login Data Provided is wrong"
        };

        context.Result = new UnauthorizedObjectResult(details);

        context.ExceptionHandled = true;
    }
}