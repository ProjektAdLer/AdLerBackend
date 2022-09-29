using AdLerBackend.Application.Common.InternalUseCases.GetLearningElementLmsInformation;
using AdLerBackend.Application.Common.Responses.Course;
using AdLerBackend.Application.Common.Responses.LearningElements;
using AdLerBackend.Application.Common.Responses.LMSAdapter;
using AdLerBackend.Application.LearningElement.GetLearningElementSource;
using AdLerBackend.Application.LearningElement.GetLearningElementSource.GetH5PFilePath;
using FluentAssertions;
using MediatR;
using NSubstitute;

namespace AdLerBackend.Application.UnitTests.LearningElements;

public class GetLearningElementSourceTest
{
    private IMediator _mediator;

    [SetUp]
    public void Setup()
    {
        _mediator = Substitute.For<IMediator>();
    }

    [TestCase("resource")]
    [TestCase("url")]
    public async Task GetLearningElementSource_Valid_GenericElements(string resourceType)
    {
        // Arrange
        var systemUnderTest = new GetLearningElementSourceHandler(_mediator);

        _mediator.Send(Arg.Any<GetLearningElementLmsInformationCommand>())
            .Returns(new GetLearningElementLmsInformationResponse
            {
                LearningElementData = new Modules
                {
                    ModName = resourceType,
                    Contents = new List<FileContents>
                    {
                        new()
                        {
                            fileUrl = "testURL"
                        }
                    }
                }
            });

        // Act
        var result = await systemUnderTest.Handle(new GetLearningElementSourceCommand
        {
            CourseId = 1,
            ElementId = 1,
            WebServiceToken = "token"
        }, CancellationToken.None);

        // Assert
        result.FilePath.Should().Be("testURL" + "&token=token");
    }

    [Test]
    public async Task GetLearningElementSource_Valid_H5PElements()
    {
        // Arrange
        var systemUnderTest = new GetLearningElementSourceHandler(_mediator);

        _mediator.Send(Arg.Any<GetLearningElementLmsInformationCommand>())
            .Returns(new GetLearningElementLmsInformationResponse
            {
                LearningElementData = new Modules
                {
                    ModName = "h5pactivity",
                    Contents = new List<FileContents>
                    {
                        new()
                        {
                            fileUrl = "testURL"
                        }
                    }
                }
            });

        _mediator.Send(Arg.Any<GetH5PFilePathCommand>())
            .Returns(new GetLearningElementSourceResponse
            {
                FilePath = "testURL"
            });

        // Act
        var result = await systemUnderTest.Handle(new GetLearningElementSourceCommand
        {
            CourseId = 1,
            ElementId = 1,
            WebServiceToken = "token"
        }, CancellationToken.None);

        // Assert
        result.FilePath.Should().Be("testURL");
    }

    [Test]
    public async Task GetLearningElementSource_InvalidRessourceName_Throws()
    {
        // Arrange
        var systemUnderTest = new GetLearningElementSourceHandler(_mediator);

        _mediator.Send(Arg.Any<GetLearningElementLmsInformationCommand>())
            .Returns(new GetLearningElementLmsInformationResponse
            {
                LearningElementData = new Modules
                {
                    ModName = "h5pactivity123456789",
                    Contents = new List<FileContents>
                    {
                        new()
                        {
                            fileUrl = "testURL"
                        }
                    }
                }
            });

        _mediator.Send(Arg.Any<GetH5PFilePathCommand>())
            .Returns(new GetLearningElementSourceResponse
            {
                FilePath = "testURL"
            });

        // Act
        // Assert
        Assert.ThrowsAsync<Exception>(async () => await systemUnderTest.Handle(new GetLearningElementSourceCommand
        {
            CourseId = 1,
            ElementId = 1,
            WebServiceToken = "token"
        }, CancellationToken.None));
    }
}