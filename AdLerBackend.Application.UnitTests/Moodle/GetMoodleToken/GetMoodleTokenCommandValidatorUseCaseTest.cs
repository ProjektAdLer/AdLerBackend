using AdLerBackend.Application.LMS.GetLMSToken;
using FluentValidation.TestHelper;

namespace AdLerBackend.Application.UnitTests.Moodle.GetMoodleToken;

public class MoodleLoginCommandValidatorTest
{
    private GetLmsTokenCommandValidator _validator;

    [SetUp]
    public void Setup()
    {
        _validator = new GetLmsTokenCommandValidator();
    }

    [Test]
    public void Should_have_error_when_username_is_null()
    {
        var command = new GetLmsTokenCommand
        {
            UserName = null,
            Password = "password"
        };

        var result = _validator.TestValidate(command);

        Assert.Multiple(() =>
        {
            Assert.That(result.IsValid, Is.False);
            Assert.That(result.Errors, Has.Count.EqualTo(1));
        });
        Assert.That(result.Errors[0].ErrorMessage, Is.EqualTo("Username is required"));
    }

    [Test]
    public void Should_have_error_when_username_is_empty()
    {
        var command = new GetLmsTokenCommand
        {
            UserName = "",
            Password = "password"
        };

        var result = _validator.TestValidate(command);

        Assert.Multiple(() =>
        {
            Assert.That(result.IsValid, Is.False);
            Assert.That(result.Errors, Has.Count.EqualTo(1));
        });
        Assert.That(result.Errors[0].ErrorMessage, Is.EqualTo("Username is required"));
    }

    [Test]
    public void Should_have_error_when_password_is_null()
    {
        var command = new GetLmsTokenCommand
        {
            UserName = "username",
            Password = null
        };

        var result = _validator.TestValidate(command);

        Assert.Multiple(() =>
        {
            Assert.That(result.IsValid, Is.False);
            Assert.That(result.Errors, Has.Count.EqualTo(1));
        });
        Assert.That(result.Errors[0].ErrorMessage, Is.EqualTo("Password is required"));
    }

    [Test]
    public void Should_have_error_when_password_is_empty()
    {
        var command = new GetLmsTokenCommand
        {
            UserName = "username",
            Password = ""
        };

        var result = _validator.TestValidate(command);

        Assert.Multiple(() =>
        {
            Assert.That(result.IsValid, Is.False);
            Assert.That(result.Errors, Has.Count.EqualTo(1));
        });
        Assert.That(result.Errors[0].ErrorMessage, Is.EqualTo("Password is required"));
    }
}