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
    private IFileAccess _fileAccess;
    private ILMS _ilms;
    private ISerialization _serialization;
    private IWorldRepository _worldRepository;

    [SetUp]
    public void Setup()
    {
        _ilms = Substitute.For<ILMS>();
        _worldRepository = Substitute.For<IWorldRepository>();
        _fileAccess = Substitute.For<IFileAccess>();
        _serialization = new SerializationService();
    }

    [Test]
    public async Task GetLearningElementLmsInformation_Valid_RetunsCourseModule()
    {
        // Arrange
        var systemUnderTest =
            new GetAllElementsFromLmsUseCase(_worldRepository, _fileAccess, _serialization, _ilms);

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
                Elements = new List<Application.Common.Responses.World.Element>
                {
                    new()
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


        _ilms.GetWorldContentAsync(Arg.Any<string>(), Arg.Any<int>()).Returns(new[]
        {
            new WorldContent
            {
                Modules = new List<Modules>
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

        Assert.NotNull(result);
    }


    [Test]
    public async Task GetLearningElementLmsInformation_CourseNotFound_Throws()
    {
        // Arrange
        var systemUnderTest =
            new GetAllElementsFromLmsUseCase(_worldRepository, _fileAccess, _serialization, _ilms);

        var fakeATF = new WorldAtfResponse
        {
            World = new Application.Common.Responses.World.World
            {
                Elements = new List<Application.Common.Responses.World.Element>
                {
                    new()
                    {
                        ElementId = 1337
                    }
                }
            }
        };

        _worldRepository.GetAsync(Arg.Any<int>()).Returns((WorldEntity?) null);


        _ilms.GetWorldContentAsync(Arg.Any<string>(), Arg.Any<int>()).Returns(new[]
        {
            new WorldContent
            {
                Modules = new List<Modules>
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
    }
}