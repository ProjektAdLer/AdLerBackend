using AdLerBackend.API.Common;
using AdLerBackend.API.Filters;
using AdLerBackend.Application.Common.Exceptions;
using AdLerBackend.Application.Common.Exceptions.LMSAdapter;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;

namespace AdLerBackend.API.UnitTests.Filters;

public class ApiExceptionFilterAttributeTest
{
    [SetUp]
    public void Setup()
    {
        var actionContext = new ActionContext
        {
            HttpContext = new DefaultHttpContext(),
            RouteData = new RouteData(),
            ActionDescriptor = new ActionDescriptor()
        };

        _context = new ExceptionContext(actionContext, new List<IFilterMetadata>());
        _filter = new ApiExceptionFilterAttribute();
    }

    [Test]
    public void ApiExceptionFilterAttribute_Should_HandleGenericLmsException()
    {
        // Arrange
        var exception = new LmsException("Test exception");
        _context.Exception = exception;

        // Act
        _filter.OnException(_context);

        // Assert
        var result = _context.Result as ObjectResult;
        var resultValue = result!.Value;
        Assert.IsInstanceOf<ProblemDetails>(resultValue);
    }

    [Test]
    public void ApiExceptionFilterAttribute_Should_Handle_InvalidMoodleLogin()
    {
        // Arrange
        _context.Exception = new InvalidLMSLoginException();

        // Act
        _filter.OnException(_context);

        // Assert
        var result = _context.Result as ObjectResult;
        var resultValue = result!.Value;

        Assert.IsInstanceOf<ProblemDetails>(resultValue);
    }

    [Test]
    public void ApiExceptionFilterAttribute_Should_Handle_ValidationException()
    {
        // Arrange
        _context.Exception = new ValidationException();

        // Act
        _filter.OnException(_context);

        // Assert
        Assert.That(_context.Result, Is.InstanceOf<ObjectResult>());
        var result = (ObjectResult) _context.Result!;

        Assert.That(result.Value, Is.InstanceOf<ValidationProblemDetails>());
        var resultValue = (ValidationProblemDetails) result.Value!;

        Assert.That(resultValue.Type, Is.EqualTo(ErrorCodes.ValidationError));
    }

    [Test]
    public void ApiExceptionFilterAttribute_Should_Handle_UnknownException()
    {
        // Arrange
        _context.Exception = new Exception("Unknown Exception");

        // Act
        _filter.OnException(_context);

        // Assert
        var result = _context.Result as ObjectResult;
        var resultValue = result!.Value as ProblemDetails;

        Assert.That(resultValue, Is.InstanceOf<ProblemDetails>());
        resultValue!.Type.Should().Be(ErrorCodes.UnknownError);
    }

    [Test]
    public void ApiExceptionFilterAttributeShouldHandleTokenException()
    {
        // Arrange
        _context.Exception = new InvalidTokenException();

        // Act
        _filter.OnException(_context);

        // Assert
        var result = _context.Result as ObjectResult;
        var resultValue = result!.Value as ProblemDetails;

        Assert.IsInstanceOf<ProblemDetails>(resultValue);
        if (resultValue != null) resultValue.Type.Should().Be(ErrorCodes.LmsTokenInvalid);
    }

    [Test]
    public void ApiExceptionFilterAttribute_NotFoundException_ShouldReturnProblemDetails()
    {
        // Arrange
        _context.Exception = new NotFoundException();

        // Act
        _filter.OnException(_context);

        // Assert
        var result = _context.Result as ObjectResult;
        var resultValue = result!.Value;

        Assert.IsInstanceOf<ProblemDetails>(resultValue);
    }

    [Test]
    public void ApiExceptionFilterAttribute_ForbiddenAccessException_ShouldReturnNotFoundObjectResult()
    {
        // Arrange
        _context.Exception = new ForbiddenAccessException("Test");

        // Act
        _filter.OnException(_context);

        // Assert
        var result = _context.Result as ObjectResult;
        var resultValue = result!.Value;

        Assert.IsInstanceOf<ProblemDetails>(resultValue);
    }

    [Test]
    public void ApiExceptionFilterAttribute_CourseCreationException()
    {
        // Arrange
        _context.Exception = new WorldCreationException("Test");

        // Act
        _filter.OnException(_context);

        // Assert
        var result = _context.Result as ObjectResult;
        var resultValue = result!.Value;

        Assert.IsInstanceOf<ProblemDetails>(resultValue);
    }

#pragma warning disable CS8618
    private ExceptionContext _context;
    private ApiExceptionFilterAttribute _filter;
#pragma warning restore CS8618
}