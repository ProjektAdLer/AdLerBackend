using AdLerBackend.Application.Common.Exceptions;
using AdLerBackend.Application.Common.InternalUseCases.GetAllElementsFromLms;
using AdLerBackend.Application.Common.InternalUseCases.GetLearningElement;
using AdLerBackend.Application.Common.Responses.World;
using FluentAssertions;
using MediatR;
using NSubstitute;

namespace AdLerBackend.Application.UnitTests.Common.InternalUseCases;

public class GetLearningElementUseCaseTest
{
    private IMediator _mediatorMock;

    [SetUp]
    public void Setup()
    {
        _mediatorMock = Substitute.For<IMediator>();
    }

    [Test]
    public async Task GetLearningElement_Valid_ReturnsLearningElement()
    {
        // Arrange
        _mediatorMock.Send(Arg.Any<GetAllElementsFromLmsCommand>()).Returns(
            new GetAllElementsFromLmsWithAdLerIdResponse
            {
                ElementAggregations = new List<AdLerLmsElementAggregation>
                {
                    new()
                    {
                        AdLerElement = new BaseElement
                        {
                            ElementId = 1
                        }
                    },
                    new()
                    {
                        AdLerElement = new BaseElement
                        {
                            ElementId = 2
                        }
                    }
                },
                LmsCourseId = 123,
                AdLerWorldId = 1234
            }
        );
        var systemUnderTest = new GetLearningElementUseCase(_mediatorMock);

        // Act
        var result = await systemUnderTest.Handle(new GetLearningElementCommand
        {
            ElementId = 1,
            CanBeLocked = false
        }, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.AdLerElement.ElementId.Should().Be(1);
    }

    [Test]
    public async Task GetLearningElement_ElementNotFound_ThrowsNotFoundException()
    {
        // Arrange
        _mediatorMock.Send(Arg.Any<GetAllElementsFromLmsCommand>()).Returns(
            new GetAllElementsFromLmsWithAdLerIdResponse
            {
                ElementAggregations = new List<AdLerLmsElementAggregation>
                {
                    new()
                    {
                        AdLerElement = new BaseElement
                        {
                            ElementId = 1
                        }
                    },
                    new()
                    {
                        AdLerElement = new BaseElement
                        {
                            ElementId = 2
                        }
                    }
                },
                LmsCourseId = 123,
                AdLerWorldId = 1234
            }
        );
        var systemUnderTest = new GetLearningElementUseCase(_mediatorMock);

        // Act
        Func<Task> act = async () => await systemUnderTest.Handle(new GetLearningElementCommand
        {
            ElementId = 3,
            CanBeLocked = false
        }, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Test]
    public async Task GetLearningElement_ElementIsLockedAndCannotBeAccessed_ThrowsForbiddenAccessException()
    {
        // Arrange
        _mediatorMock.Send(Arg.Any<GetAllElementsFromLmsCommand>()).Returns(
            new GetAllElementsFromLmsWithAdLerIdResponse
            {
                ElementAggregations = new List<AdLerLmsElementAggregation>
                {
                    new()
                    {
                        AdLerElement = new BaseElement
                        {
                            ElementId = 1
                        },
                        IsLocked = true
                    },
                    new()
                    {
                        AdLerElement = new BaseElement
                        {
                            ElementId = 2
                        }
                    }
                },
                LmsCourseId = 123,
                AdLerWorldId = 1234
            }
        );
        var systemUnderTest = new GetLearningElementUseCase(_mediatorMock);

        // Act
        Func<Task> act = async () => await systemUnderTest.Handle(new GetLearningElementCommand
        {
            ElementId = 1,
            CanBeLocked = false
        }, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ForbiddenAccessException>();
    }
}