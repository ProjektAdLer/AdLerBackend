using AdLerBackend.Application.Common.InternalUseCases.GetLearningElement;
using AdLerBackend.Application.Common.Responses.Elements;
using AdLerBackend.Application.Common.Responses.LMSAdapter;
using AdLerBackend.Application.Common.Responses.World;
using AdLerBackend.Application.Element.GetElementSource;
using AdLerBackend.Application.Element.GetElementSource.GetH5PFilePath;
using FluentAssertions;
using MediatR;
using NSubstitute;

namespace AdLerBackend.Application.UnitTests.LearningElements;

public class GetLearningElementSourceUseCaseTest
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
        var systemUnderTest = new GetElementSourceUseCase(_mediator);

        _mediator.Send(Arg.Any<GetLearningElementCommand>()).Returns(
            new AdLerLmsElementAggregation
            {
                IsLocked = false,
                AdLerElement = new BaseElement
                {
                    ElementId = 1
                },
                LmsModule = new LmsModule
                {
                    contextid = 1,
                    Id = 1,
                    Name = "name",
                    ModName = resourceType,
                    Contents = new List<FileContents>
                    {
                        new()
                        {
                            fileUrl = "testURL"
                        }
                    }
                }
            }
        );

        // Act
        var result = await systemUnderTest.Handle(new GetElementSourceCommand
        {
            WorldId = 1,
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
        var systemUnderTest = new GetElementSourceUseCase(_mediator);

        _mediator.Send(Arg.Any<GetLearningElementCommand>()).Returns(
            new AdLerLmsElementAggregation
            {
                IsLocked = false,
                AdLerElement = new BaseElement
                {
                    ElementId = 1
                },
                LmsModule = new LmsModule
                {
                    contextid = 1,
                    Id = 1,
                    Name = "name",
                    ModName = "h5pactivity",
                    Contents = new List<FileContents>
                    {
                        new()
                        {
                            fileUrl = "testURL"
                        }
                    }
                }
            }
        );

        _mediator.Send(Arg.Any<GetH5PFilePathCommand>())
            .Returns(new GetElementSourceResponse
            {
                FilePath = "testURL"
            });

        // Act
        var result = await systemUnderTest.Handle(new GetElementSourceCommand
        {
            WorldId = 1,
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
        var systemUnderTest = new GetElementSourceUseCase(_mediator);

        _mediator.Send(Arg.Any<GetLearningElementCommand>()).Returns(
            new AdLerLmsElementAggregation
            {
                IsLocked = false,
                AdLerElement = new BaseElement
                {
                    ElementId = 1
                },
                LmsModule = new LmsModule
                {
                    contextid = 1,
                    Id = 1,
                    Name = "name",
                    ModName = "h5pactivity123456789"
                }
            }
        );

        _mediator.Send(Arg.Any<GetH5PFilePathCommand>())
            .Returns(new GetElementSourceResponse
            {
                FilePath = "testURL"
            });

        // Act
        // Assert
        Assert.ThrowsAsync<NotImplementedException>(async () => await systemUnderTest.Handle(
            new GetElementSourceCommand
            {
                WorldId = 1,
                ElementId = 1,
                WebServiceToken = "token"
            }, CancellationToken.None));
    }
}