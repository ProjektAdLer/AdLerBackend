using AdLerBackend.Application.Common.InternalUseCases.GetAllElementsFromLms;
using AdLerBackend.Application.Common.InternalUseCases.GetElementLmsInformation;
using AdLerBackend.Application.Common.Responses.LMSAdapter;
using AdLerBackend.Application.Common.Responses.World;
using MediatR;
using NSubstitute;

namespace AdLerBackend.Application.UnitTests.Common.InternalUseCases;

public class GetElementLmsInformationUseCaseTest
{
    private IMediator _mediator;

    [SetUp]
    public void Setup()
    {
        _mediator = Substitute.For<IMediator>();
    }

    [Test]
    public async Task Handle_Valid_ReturnsSingleModule()
    {
        // Arrange
        var systemUnderTest = new GetLearningElementLmsInformationUseCase(_mediator);
        _mediator.Send(Arg.Any<GetAllElementsFromLmsCommand>()).Returns(
            new GetAllElementsFromLmsWithAdLerIdResponse
            {
                LmsCourseId = 1,
                ModulesWithAdLerId = new List<ModuleWithId>
                {
                    new()
                    {
                        AdLerId = 2,
                        LmsModule = new Modules
                        {
                            Id = 3,
                            Name = "TestName",
                            ModName = "ModName"
                        }
                    }
                }
            }
        );


        // Act
        var result = systemUnderTest.Handle(new GetElementLmsInformationCommand
        {
            ElementId = 2,
            WorldId = 1
        }, CancellationToken.None).Result;

        // Assert
        Assert.That(result.ElementData.Id, Is.EqualTo(3));
        Assert.That(result.ElementData.ModName, Is.EqualTo("ModName"));
    }

    [Test]
    public async Task Handle_ModuleNotExisting_ThrowsOperationException()
    {
        // Arrange
        var systemUnderTest = new GetLearningElementLmsInformationUseCase(_mediator);
        _mediator.Send(Arg.Any<GetAllElementsFromLmsCommand>()).Returns(
            new GetAllElementsFromLmsWithAdLerIdResponse
            {
                LmsCourseId = 1,
                ModulesWithAdLerId = new List<ModuleWithId>
                {
                    new()
                    {
                        AdLerId = 2,
                        LmsModule = new Modules
                        {
                            Id = 3,
                            Name = "TestName",
                            ModName = "ModName"
                        }
                    }
                }
            }
        );

        // Act
        var result = systemUnderTest.Handle(new GetElementLmsInformationCommand
        {
            ElementId = 4,
            WorldId = 1
        }, CancellationToken.None);

        // Assert
        Assert.ThrowsAsync<InvalidOperationException>(async () => await result);
    }
}