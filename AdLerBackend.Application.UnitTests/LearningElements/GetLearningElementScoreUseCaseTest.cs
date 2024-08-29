using AdLerBackend.Application.Common.Interfaces;
using AdLerBackend.Application.Common.InternalUseCases.GetLearningElement;
using AdLerBackend.Application.Common.Responses.LMSAdapter;
using AdLerBackend.Application.Common.Responses.World;
using AdLerBackend.Application.Element.GetElementScore;
using FluentAssertions;
using MediatR;
using NSubstitute;

namespace AdLerBackend.Application.UnitTests.LearningElements;

public class GetLearningElementScoreUseCaseTest
{
    private ILMS _lms;
    private IMediator _mediator;

    [SetUp]
    // ANF-ID: [BPG16]
    public void Setup()
    {
        _mediator = Substitute.For<IMediator>();
        _lms = Substitute.For<ILMS>();
    }

    [TestCase("h5pactivity", true)]
    [TestCase("h5pactivity", false)]
    [TestCase("url", false)]
    [TestCase("resource", false)]
    public async Task GetLearningElementScore_Valid_GetsScoreFromApi(string modname, bool success)
    {
        // Arrange
        var systemUnderTest = new GetElementScoreUseCase(_mediator, _lms);
        _lms.GetElementScoreFromPlugin(Arg.Any<string>(), Arg.Any<int>())
            .Returns(
                success
            );

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
                    ModName = modname
                }
            }
        );


        // Act
        var result = await systemUnderTest.Handle(new GetElementScoreCommand
        {
            ElementId = 1,
            LearningWorldId = 1,
            WebServiceToken = "token"
        }, CancellationToken.None);

        // Assert
        result.Success.Should().Be(success);
    }

    // ANF-ID: [BPG16]
    [Test]
    public async Task GetLearningElementScore_ElementIsLocke_ReturnsFalse()
    {
        // Arrange
        var systemUnderTest = new GetElementScoreUseCase(_mediator, _lms);
        _mediator.Send(Arg.Any<GetLearningElementCommand>()).Returns(
            new AdLerLmsElementAggregation
            {
                IsLocked = true,
                AdLerElement = new BaseElement
                {
                    ElementId = 1
                },
                LmsModule = new LmsModule
                {
                    contextid = 1,
                    Id = 1,
                    Name = "name",
                    ModName = "h5pactivity"
                }
            }
        );

        // Act
        var result = await systemUnderTest.Handle(new GetElementScoreCommand
        {
            ElementId = 1,
            LearningWorldId = 1,
            WebServiceToken = "token"
        }, CancellationToken.None);

        // Assert
        result.Success.Should().BeFalse();
    }
}