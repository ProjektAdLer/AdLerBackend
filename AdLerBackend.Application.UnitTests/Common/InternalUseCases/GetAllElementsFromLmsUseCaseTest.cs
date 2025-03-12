using AdLerBackend.Application.Common.Exceptions;
using AdLerBackend.Application.Common.Interfaces;
using AdLerBackend.Application.Common.InternalUseCases.GetAllElementsFromLms;
using AdLerBackend.Application.Common.Responses.LMSAdapter;
using AdLerBackend.Application.Common.Responses.World;
using AdLerBackend.Domain.Entities;
using AdLerBackend.Domain.UnitTests.TestingUtils;
using AdLerBackend.Infrastructure.Services;
using Newtonsoft.Json;
using NSubstitute;

namespace AdLerBackend.Application.UnitTests.Common.InternalUseCases;

public class GetAllElementsFromLmsUseCaseTest
{
    private ILMS _lms;
    private ISerialization _serialization;
    private IWorldRepository _worldRepository;

    [SetUp]
    public void Setup()
    {
        _lms = Substitute.For<ILMS>();
        _worldRepository = Substitute.For<IWorldRepository>();
        _serialization = new SerializationService();
    }

    [Test]
    public async Task GetLearningElementLmsInformation_Valid_RetunsCourseModule()
    {
        // Arrange
        var systemUnderTest =
            new GetAllElementsFromLmsUseCase(_worldRepository, _serialization, _lms);

        var fakeAtf = new WorldAtfResponse
        {
            FileVersion = "FileVersion",
            AmgVersion = "000",
            Author = "Author",
            Language = "Language",
            World = new Application.Common.Responses.World.World
            {
                Spaces = new List<Space>
                {
                    new()
                    {
                        SpaceId = 1234,
                        SpaceName = "spaceName"
                    }
                },
                Elements = new List<BaseElement>
                {
                    new Application.Common.Responses.World.Element
                    {
                        ElementId = 1337
                    }
                }
            }
        };

        var worldEntity = new WorldEntity(
            "name",
            new List<H5PLocationEntity>
            {
                H5PLocationEntityFactory.CreateH5PLocationEntity("path", 4, 3)
            },
            1234,
            JsonConvert.SerializeObject(fakeAtf),
            1,
            2
        );


        _worldRepository.GetAsync(Arg.Any<int>()).Returns(worldEntity);


        _lms.GetWorldContentAsync(Arg.Any<string>(), Arg.Any<int>()).Returns(new[]
        {
            new LMSWorldContentResponse
            {
                Modules = new List<LmsModule>
                {
                    new()
                    {
                        Name = "searchedFileName",
                        contextid = 123
                    }
                }
            }
        });

        // Act

        var result =
            await systemUnderTest.Handle(new GetAllElementsFromLmsCommand
            {
                WorldId = 1,
                WebServiceToken = "token"
            }, CancellationToken.None);


        // Assert

        Assert.That(result, Is.Not.Null);
    }


    [Test]
    public Task GetLearningElementLmsInformation_CourseNotFound_Throws()
    {
        // Arrange
        var systemUnderTest =
            new GetAllElementsFromLmsUseCase(_worldRepository, _serialization, _lms);

        _worldRepository.GetAsync(Arg.Any<int>()).Returns((WorldEntity?) null);


        _lms.GetWorldContentAsync(Arg.Any<string>(), Arg.Any<int>()).Returns(new[]
        {
            new LMSWorldContentResponse
            {
                Modules = new List<LmsModule>
                {
                    new()
                    {
                        contextid = 123
                    }
                }
            }
        });

        // Act
        //Assert
        Assert.ThrowsAsync<NotFoundException>(async () =>
            await systemUnderTest.Handle(new GetAllElementsFromLmsCommand
            {
                WorldId = 1,
                WebServiceToken = "token"
            }, CancellationToken.None));
        return Task.CompletedTask;
    }
}