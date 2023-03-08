using AdLerBackend.Application.LMS.GetLMSToken;
using FluentValidation.TestHelper;

namespace AdLerBackend.Application.UnitTests.Moodle.GetMoodleToken;

public class MoodleLoginCommandValidatorTest
{
    private GetLMSTokenCommandValidator _validator;

    [SetUp]
    public void Setup()
    {
        _validator = new GetLMSTokenCommandValidator();
    }

    [Test]
    public void Should_have_error_when_username_is_null()
    {
        var command = new GetLMSTokenCommand
        {
            UserName = null,
            Password = "password"
        };

        var result = _validator.TestValidate(command);

        Assert.IsFalse(result.IsValid);
        Assert.That(result.Errors.Count, Is.EqualTo(1));
        Assert.That(result.Errors[0].ErrorMessage, Is.EqualTo("Username is required"));
    }

    [Test]
    public void Should_have_error_when_username_is_empty()
    {
        var command = new GetLMSTokenCommand
        {
            UserName = "",
            Password = "password"
        };

        var result = _validator.TestValidate(command);

        Assert.IsFalse(result.IsValid);
        Assert.That(result.Errors.Count, Is.EqualTo(1));
        Assert.That(result.Errors[0].ErrorMessage, Is.EqualTo("Username is required"));
    }

    [Test]
    public void Should_have_error_when_password_is_null()
    {
        var command = new GetLMSTokenCommand
        {
            UserName = "username",
            Password = null
        };

        var result = _validator.TestValidate(command);

        Assert.IsFalse(result.IsValid);
        Assert.That(result.Errors.Count, Is.EqualTo(1));
        Assert.That(result.Errors[0].ErrorMessage, Is.EqualTo("Password is required"));
    }

    [Test]
    public void Should_have_error_when_password_is_empty()
    {
        var command = new GetLMSTokenCommand
        {
            UserName = "username",
            Password = ""
        };

        var result = _validator.TestValidate(command);

        Assert.IsFalse(result.IsValid);
        Assert.That(result.Errors.Count, Is.EqualTo(1));
        Assert.That(result.Errors[0].ErrorMessage, Is.EqualTo("Password is required"));
    }
}