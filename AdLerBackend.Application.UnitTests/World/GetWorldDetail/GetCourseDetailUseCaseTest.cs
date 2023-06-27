using AdLerBackend.Application.Common.Exceptions;
using AdLerBackend.Application.Common.Interfaces;
using AdLerBackend.Application.Common.Responses.World;
using AdLerBackend.Application.World.GetWorldDetail;
using AdLerBackend.Domain.Entities;
using AdLerBackend.Domain.UnitTests.TestingUtils;
using AdLerBackend.Infrastructure.Services;
using AutoBogus;
using Newtonsoft.Json;
using NSubstitute;

#pragma warning disable CS8618

namespace AdLerBackend.Application.UnitTests.World.GetWorldDetail;

public class GetWorldDetailUseCaseTest
{
    private IFileAccess _fileAccess;
    private ISerialization _serialization;
    private IWorldRepository _worldRepository;

    [SetUp]
    public void Setup()
    {
        _worldRepository = Substitute.For<IWorldRepository>();
        _fileAccess = Substitute.For<IFileAccess>();
        _serialization = new SerializationService();
    }

    [Test]
    public async Task Handle_GivenValidId_ReturnsCourseDetail()
    {
        // Arrange
        var request = new GetWorldDetailCommand
        {
            WorldId = 1,
            WebServiceToken = "testToken"
        };

        var mockedDsl = AutoFaker.Generate<WorldAtfResponse>();
        mockedDsl.World.Elements = new List<Application.Common.Responses.World.Element>
        {
            new()
            {
                ElementId = 1,
                ElementCategory = "h5p"
            },
            new()
            {
                ElementId = 2,
                ElementCategory = "h5p"
            }
        };

        var worldEntity = WorldEntityFactory.CreateWorldEntity();
        worldEntity.Id = 1;
        worldEntity.H5PFilesInCourse = new List<H5PLocationEntity>
        {
            H5PLocationEntityFactory.CreateH5PLocationEntity(Path.Combine("some", "path1")),
            H5PLocationEntityFactory.CreateH5PLocationEntity(Path.Combine("some", "path2"))
        };

        worldEntity.AtfJson = JsonConvert.SerializeObject(mockedDsl);

        _worldRepository.GetAsync(Arg.Any<int>()).Returns(worldEntity);


        var systemUnderTest = new GetWorldDetailUseCase(_worldRepository, _fileAccess, _serialization);

        // Act
        var result = await systemUnderTest.Handle(request, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.That(result.World.Elements.Count, Is.EqualTo(2));
    }

    [Test]
    public Task Handle_CourseNotFound_ThrowsNotFound()
    {
        // Arrange
        var request = new GetWorldDetailCommand
        {
            WorldId = 1,
            WebServiceToken = "testToken"
        };

        WorldEntity? courseDatabaseResponse = null;

        _worldRepository.GetAsync(Arg.Any<int>()).Returns(courseDatabaseResponse);

        var systemUnderTest = new GetWorldDetailUseCase(_worldRepository, _fileAccess, _serialization);

        // Act
        Assert.ThrowsAsync<NotFoundException>(async () =>
            await systemUnderTest.Handle(request, CancellationToken.None));
        return Task.CompletedTask;
    }
}