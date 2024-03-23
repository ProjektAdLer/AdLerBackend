// using AdLerBackend.Application.Adaptivity.AnswerAdaptivityQuestion;
// using AdLerBackend.Application.Common.Interfaces;
// using AdLerBackend.Application.Common.InternalUseCases.GetLearningElement;
// using AdLerBackend.Application.Common.Responses.LMSAdapter;
// using AdLerBackend.Application.Common.Responses.World;
// using AdLerBackend.Domain.Entities;
// using AutoBogus;
// using MediatR;
// using NSubstitute;
//
// namespace AdLerBackend.Application.UnitTests.Adaptivity.AnswerAdaptivityQuestion;
//
// public class AnswerAdaptivityQuestionUseCaseTests
// {
//     private ILMS _ilms;
//     private IMediator _mediator;
//     private IWorldRepository _worldRepository;
//     private ISerialization _serialization;
//     private IAutoFaker _faker;
//
//     [SetUp]
//     public void Setup()
//     {
//         _ilms = Substitute.For<ILMS>();
//         _mediator = Substitute.For<IMediator>();
//         _worldRepository = Substitute.For<IWorldRepository>();
//         _serialization = Substitute.For<ISerialization>();
//         _faker = AutoFaker.Create();
//     }
//     
//     [Test]
//     public async Task Handler_AnswersAdaptivityQuestion()
//     {
//         // Arrange
//
//         var systemUnderTest = new AnswerAdaptivityQuestionHandler(_ilms, _mediator, _worldRepository, _serialization);
//         _mediator.Send(Arg.Any<GetLearningElementCommand>())
//             .Returns(new AdLerLmsElementAggregation()
//             {
//                 IsLocked = false,
//                 AdLerElement = new AdaptivityElement()
//                 {
//                     ElementId = 1,
//                     
//                 },
//                 LmsModule = new LmsModule()
//                 {
//                     contextid = 1,
//                     Id = 1,
//                 }
//             });
//         
//         _worldRepository.GetAsync(Arg.Any<int>())
//             .Returns(_faker.Generate<WorldEntity>());
//         
//         _serialization.GetObjectFromJsonString<WorldAtfResponse>(Arg.Any<string>())
//             .Returns(new WorldAtfResponse()
//             {
//                 World = new()
//                 {
//                     Elements = new List<BaseElement>()
//                     {
//                         new AdaptivityElement()
//                         {
//                             ElementId = 1,
//                             AdaptivityContent = new AdaptivityContent()
//                             {
//                                 AdaptivityTasks = new()
//                                 {
//                                     new()
//                                     {
//                                         TaskId = 1,
//                                         AdaptivityQuestions = new()
//                                         {
//                                             new AdaptivityQuestion()
//                                             {
//                                                 QuestionId = 1,
//                                                 QuestionUuid = new Guid()
//                                             }
//                                         }
//                                     }
//                                 }
//                             }
//                         }
//                     }
//                 }
//             });
//         
//         // Act
//         var result = await systemUnderTest.Handle(new AnswerAdaptivityQuestionCommand
//         {
//             ElementId = 1,
//             QuestionId = 1,
//         }, CancellationToken.None);
//         
//         // Assert
//         // Assert the result
//     }
// }