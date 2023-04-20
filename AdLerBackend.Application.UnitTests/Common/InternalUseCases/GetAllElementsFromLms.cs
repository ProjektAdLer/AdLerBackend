using AdLerBackend.Application.Common.Exceptions;
using AdLerBackend.Application.Common.Interfaces;
using AdLerBackend.Application.Common.InternalUseCases.GetAllElementsFromLms;
using AdLerBackend.Application.Common.Responses.Course;
using AdLerBackend.Application.Common.Responses.LMSAdapter;
using AdLerBackend.Domain.Entities;
using AdLerBackend.Domain.UnitTests.TestingUtils;
using NSubstitute;

namespace AdLerBackend.Application.UnitTests.Common.InternalUseCases;

public class GetAllElementsFromLms
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
        _serialization = Substitute.For<ISerialization>();
    }

    [Test]
    public async Task GetLearningElementLmsInformation_Valid_RetunsCourseModule()
    {
        // Arrange
        var systemUnderTest =
            new GetAllElementsFromLmsHandler(_worldRepository, _fileAccess, _serialization, _ilms);

        var worldEntity = new WorldEntity(
            "name",
            new List<H5PLocationEntity>
            {
                H5PLocationEntityFactory.CreateH5PLocationEntity("path", 4, 3)
            },
            "asd",
            1234,
            2
        );


        _worldRepository.GetAsync(Arg.Any<int>()).Returns(worldEntity);


        _fileAccess.GetReadFileStream(Arg.Any<string>()).Returns(new MemoryStream());
        _serialization.GetObjectFromJsonStreamAsync<WorldDtoResponse>(Arg.Any<Stream>())
            .Returns(new WorldDtoResponse
            {
                FileVersion = "FileVersion",
                AmgVersion = "000",
                Author = "Author",
                Language = "Language",
                World = new Application.Common.Responses.Course.World
                {
                    Elements = new List<Application.Common.Responses.Course.Element>
                    {
                        new()
                        {
                            ElementId = 1337,
                            LmsElementIdentifier = new LmsElementIdentifier
                            {
                                Value = "searchedFileName"
                            },
                            ElementName = "searchedFileName"
                        }
                    }
                }
            });

        _ilms.SearchWorldsAsync(Arg.Any<string>(), Arg.Any<string>()).Returns(new LMSWorldListResponse
        {
            Courses = new List<MoodleCourse>
            {
                new()
                {
                    Id = 1
                }
            }
        });

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
            new GetAllElementsFromLmsHandler(_worldRepository, _fileAccess, _serialization, _ilms);

        _worldRepository.GetAsync(Arg.Any<int>()).Returns((WorldEntity?) null);

        _fileAccess.GetReadFileStream(Arg.Any<string>()).Returns(new MemoryStream());
        _serialization.GetObjectFromJsonStreamAsync<WorldDtoResponse>(Arg.Any<Stream>())
            .Returns(new WorldDtoResponse
            {
                World = new Application.Common.Responses.Course.World
                {
                    Elements = new List<Application.Common.Responses.Course.Element>
                    {
                        new()
                        {
                            ElementId = 1337,
                            LmsElementIdentifier = new LmsElementIdentifier
                            {
                                Value = "searchedFileName"
                            }
                        }
                    }
                }
            });

        _ilms.SearchWorldsAsync(Arg.Any<string>(), Arg.Any<string>()).Returns(new LMSWorldListResponse
        {
            Courses = new List<MoodleCourse>
            {
                new()
                {
                    Id = 1
                }
            }
        });

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
        //Assert
        Assert.ThrowsAsync<NotFoundException>(async () =>
            await systemUnderTest.Handle(new GetAllElementsFromLmsCommand
            {
                WorldId = 1,
                WebServiceToken = "token"
            }, CancellationToken.None));
    }
}