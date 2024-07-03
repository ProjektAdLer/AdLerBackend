using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using AdLerBackend.Application.Common.DTOs;
using AdLerBackend.Application.Common.DTOs.Storage;
using AdLerBackend.Application.Common.Interfaces;
using AdLerBackend.Application.Common.Responses.LMSAdapter;
using AdLerBackend.Application.Common.Responses.World;
using AdLerBackend.Application.Configuration;
using AdLerBackend.Application.LMS.GetUserData;
using AdLerBackend.Application.World.WorldManagement.UploadWorld;
using AdLerBackend.Domain.Entities;
using AdLerBackend.Domain.UnitTests.TestingUtils;
using AdLerBackend.Infrastructure.Services;
using AutoBogus;
using MediatR;
using Microsoft.Extensions.Options;
using NSubstitute;

#pragma warning disable CS8618

namespace AdLerBackend.Application.UnitTests.World.WorldManagement.UploadWorld;

public class UploadWorldUseCaseTest
{
    private IOptions<BackendConfig> _configuration;
    private IFileAccess _fileAccess;
    private ILMS _ilms;
    private ILmsBackupProcessor _lmsBackupProcessor;
    private IMediator _mediator;
    private ISerialization _serialization;
    private IWorldRepository _worldRepository;

    [SetUp]
    // ANF-ID: [BPG20]
   
    public void Setup()
    {
        _configuration = Options.Create(new BackendConfig
            {MoodleUrl = "http://localhost", AdLerEngineUrl = "http://localhost"});

        _lmsBackupProcessor = Substitute.For<ILmsBackupProcessor>();
        _mediator = Substitute.For<IMediator>();
        _fileAccess = Substitute.For<IFileAccess>();
        _worldRepository = Substitute.For<IWorldRepository>();
        _serialization = Substitute.For<ISerialization>();
        _ilms = Substitute.For<ILMS>();

        // _serialization.ClassToJsonString should have original implementation
        _serialization.ClassToJsonString(Arg.Any<object>()).Returns(x => JsonSerializer.Serialize(x.Arg<object>()));

        var mockedDsl = AutoFaker.Generate<WorldAtfResponse>();
        mockedDsl.World.Elements = new List<BaseElement>
        {
            new Application.Common.Responses.World.Element
            {
                ElementId = 1,
                ElementCategory = "h5p"
            },
            new Application.Common.Responses.World.Element
            {
                ElementId = 2,
                ElementCategory = "h5p"
            }
        };
    }

    // ANF-ID: [BPG20]
   
    [Test]
    public async Task Handle_Valid_TriggersUpload()
    {
        // Arrange
        var systemUnderTest =
            new UploadWorldUseCase(_lmsBackupProcessor, _mediator, _fileAccess, _worldRepository,
                new SerializationService(), _ilms, _configuration);

        var testGuid = Guid.NewGuid();

        _ilms.UploadCourseWorldToLMS(Arg.Any<string>(), Arg.Any<Stream>()).Returns(new LMSCourseCreationResponse
        {
            CourseLmsId = 1337,
            CourseLmsName = "TESTNAME"
        });


        _mediator.Send(Arg.Any<GetLMSUserDataCommand>()).Returns(new LMSUserDataResponse
        {
            UserEmail = "TestEmail",
            LMSUserName = "TestName",
            UserId = 1337
        });

        var fakedDsl = AutoFaker.Generate<WorldAtfResponse>();
        fakedDsl.World.Elements[0] = new Application.Common.Responses.World.Element
        {
            ElementId = 13337,
            ElementCategory = "h5p",
            ElementUuid = testGuid
        };

        _worldRepository.AddAsync(Arg.Any<WorldEntity>())
            .Returns(WorldEntityFactory.CreateWorldEntity(id: 1));

        _lmsBackupProcessor.GetWorldDescriptionFromBackup(Arg.Any<Stream>()).Returns(fakedDsl);

        _lmsBackupProcessor.GetH5PFilesFromBackup(Arg.Any<Stream>()).Returns(new List<H5PDto>

        {
            new()
            {
                H5PFile = new MemoryStream(),
                H5PUuid = testGuid.ToString()
            }
        });

        _fileAccess.StoreH5PFilesForWorld(Arg.Any<WorldStoreH5PDto>()).Returns(new Dictionary<string, string>
        {
            {testGuid.ToString(), testGuid.ToString()}
        });

        // mock memory stream with the fake atf file
        var atfStream = new MemoryStream();
        // serialize the fake atf file into a json string
        var atfJson = JsonSerializer.Serialize(fakedDsl);
        await using (var writer = new StreamWriter(atfStream, leaveOpen: true))
        {
            await writer.WriteAsync(atfJson);
        }

        atfStream.Position = 0;

        // Act
        var result = await systemUnderTest.Handle(new UploadWorldCommand
        {
            BackupFileStream = new MemoryStream(),
            ATFFileStream = atfStream,
            WebServiceToken = "testToken"
        }, CancellationToken.None);

        // Assert that AddAsync has been called with the Correct entity
        await _worldRepository.Received(1)
            .AddAsync(Arg.Is<WorldEntity>(x => x.Name == "TESTNAME"));

        Assert.That(result, Is.Not.Null);
        Assert.That(result.WorldId, Is.EqualTo(1));
        Assert.That(result.World3DUrl, Is.EqualTo("http://localhost"));
        Assert.That(result.WorldLmsUrl, Is.EqualTo("http://localhost/course/view.php?id=1337"));
    }


    // ANF-ID: [BPG20]
   
    [Test]
    public async Task Handle_ValidNoH5p_TriggersUpload()
    {
        // Arrange
        var systemUnderTest =
            new UploadWorldUseCase(_lmsBackupProcessor, _mediator, _fileAccess, _worldRepository,
                _serialization, _ilms, _configuration);

        _mediator.Send(Arg.Any<GetLMSUserDataCommand>()).Returns(new LMSUserDataResponse
        {
            UserEmail = "TestEmail",
            LMSUserName = "TestName",
            UserId = 1337
        });

        _worldRepository.AddAsync(Arg.Any<WorldEntity>())
            .Returns(WorldEntityFactory.CreateWorldEntity(id: 1));

        var fakedDsl = AutoFaker.Generate<WorldAtfResponse>();

        _ilms.UploadCourseWorldToLMS(Arg.Any<string>(), Arg.Any<Stream>()).Returns(new LMSCourseCreationResponse
        {
            CourseLmsId = 1337,
            CourseLmsName = "TESTNAME"
        });

        _lmsBackupProcessor.GetWorldDescriptionFromBackup(Arg.Any<Stream>()).Returns(fakedDsl);


        _lmsBackupProcessor.GetH5PFilesFromBackup(Arg.Any<Stream>()).Returns(new List<H5PDto>());

        _fileAccess.StoreH5PFilesForWorld(Arg.Any<WorldStoreH5PDto>()).Returns(new Dictionary<string, string>
        {
            {"path1", "path1"}
        });


        // Act
        var result = await systemUnderTest.Handle(new UploadWorldCommand
        {
            BackupFileStream = new MemoryStream(),
            ATFFileStream = new MemoryStream(),
            WebServiceToken = "testToken"
        }, CancellationToken.None);

        // Assert that AddAsync has been called with the Correct entity
        await _worldRepository.Received(1)
            .AddAsync(Arg.Is<WorldEntity>(x => x.Name == "TESTNAME"));

        Assert.That(result, Is.Not.Null);
        Assert.That(result.WorldId, Is.EqualTo(1));
        Assert.That(result.World3DUrl, Is.EqualTo("http://localhost"));
        Assert.That(result.WorldLmsUrl, Is.EqualTo("http://localhost/course/view.php?id=1337"));
    }
    
    // ANF-ID: [BPG20]
   
    [Test]
    public async Task Handle_InvalidCourseInformation_ThrowsValidationException()
    {
        // Arrange
        var systemUnderTest =
            new UploadWorldUseCase(_lmsBackupProcessor, _mediator, _fileAccess, _worldRepository,
                _serialization, _ilms, _configuration);

        var fakedValidDsl = new WorldAtfResponse()
        {

        };

        _lmsBackupProcessor.GetWorldDescriptionFromBackup(Arg.Any<Stream>()).Returns(fakedValidDsl);

        // Act & Assert
        var exception = Assert.ThrowsAsync<ValidationException>(async () => await systemUnderTest.Handle(new UploadWorldCommand
        {
            BackupFileStream = new MemoryStream(),
            ATFFileStream = new MemoryStream(),
            WebServiceToken = "testToken"
        }, CancellationToken.None));
    }
}