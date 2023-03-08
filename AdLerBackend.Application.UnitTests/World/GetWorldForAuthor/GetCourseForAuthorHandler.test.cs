using AdLerBackend.Application.Common.Exceptions;
using AdLerBackend.Application.Common.Interfaces;
using AdLerBackend.Application.Common.InternalUseCases.CheckUserPrivileges;
using AdLerBackend.Application.Common.Responses.Course;
using AdLerBackend.Application.Common.Responses.LMSAdapter;
using AdLerBackend.Application.LMS.GetUserData;
using AdLerBackend.Application.World.GetWorldsForAuthor;
using AdLerBackend.Domain.Entities;
using MediatR;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

#pragma warning disable CS8618

namespace AdLerBackend.Application.UnitTests.World.GetWorldForAuthor;

public class GetWorldForAuthorHandlerTest
{
    private IMediator _mediator;
    private IWorldRepository _worldRepository;

    [SetUp]
    public void Setup()
    {
        _worldRepository = Substitute.For<IWorldRepository>();
        _mediator = Substitute.For<IMediator>();
    }

    [Test]
    public async Task Handle_GiveUnauthorotisedUser_ShouldThrow()
    {
        // Arrange
        var request = new GetWorldsForAuthorCommand
        {
            WebServiceToken = "testToken",
            AuthorId = 1
        };

        _mediator.Send(Arg.Any<CheckUserPrivilegesCommand>()).Throws(new ForbiddenAccessException(""));

        var systemUnderTest = new GetWorldsForAuthorHandler(_worldRepository, _mediator);

        // Act

        var exception =
            Assert.ThrowsAsync<ForbiddenAccessException>(async () =>
                await systemUnderTest.Handle(request, CancellationToken.None));
    }

    [Test]
    public async Task Handle_GiveAuthorId_ShouldReturnCourses()
    {
        // Arrange
        var request = new GetWorldsForAuthorCommand
        {
            WebServiceToken = "testToken",
            AuthorId = 1
        };

        // Mock Mediatr Response for GetMoodleUserDataCommand
        var moodleUserData = new LMSUserDataResponse
        {
            IsAdmin = true,
            UserId = 1,
            LMSUserName = "userName"
        };
        _mediator.Send(Arg.Any<GetLMSUserDataCommand>()).Returns(moodleUserData);

        _worldRepository.GetAllForAuthor(1).Returns(new List<WorldEntity>
        {
            new()
            {
                Id = 1,
                Name = "Test Course",
                AuthorId = 1
            }
        });

        var systemUnderTest = new GetWorldsForAuthorHandler(_worldRepository, _mediator);

        // Act
        var result = await systemUnderTest.Handle(request, CancellationToken.None);

        // Assert
        Assert.That(result, Is.TypeOf(typeof(GetWorldOverviewResponse)));
    }
}