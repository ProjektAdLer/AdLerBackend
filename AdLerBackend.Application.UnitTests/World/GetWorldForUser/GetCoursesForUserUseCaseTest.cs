using AdLerBackend.Application.Common.Interfaces;
using AdLerBackend.Application.Common.Responses.LMSAdapter;
using AdLerBackend.Application.World.GetWorldsForUser;
using AdLerBackend.Domain.Entities;
using AdLerBackend.Domain.UnitTests.TestingUtils;
using NSubstitute;

#pragma warning disable CS8618

namespace AdLerBackend.Application.UnitTests.World.GetWorldForUser;

public class GetWorldsForUserUseCaseTest
{
    private ILMS _ilms;
    private IWorldRepository _worldRepository;


    [SetUp]
    public void Setup()
    {
        _ilms = Substitute.For<ILMS>();
        _worldRepository = Substitute.For<IWorldRepository>();
    }

    [Test]
    public async Task Handle_Valid_RetunsCoursesForUser()
    {
        // Arrange

        var systemUnderTest = new GetWorldsForUserUseCase(_ilms, _worldRepository);

        var request = new GetWorldsForUserCommand
        {
            WebServiceToken = "testToken"
        };

        _worldRepository.GetAllAsync().Returns(new List<WorldEntity>
        {
            WorldEntityFactory.CreateWorldEntity("name", new List<H5PLocationEntity>(), 1, "asdasdsd", 1, 111)
        });

        _ilms.GetWorldsForUserAsync(Arg.Any<string>()).Returns(new LMSWorldListResponse
        {
            Total = 1,
            Courses = new List<MoodleCourse>
            {
                new()
                {
                    Id = 1
                }
            }
        });

        // Act
        var result = await systemUnderTest.Handle(request, CancellationToken.None);

        // Assert
        Assert.That(result.Worlds.Count, Is.EqualTo(1));
    }
}