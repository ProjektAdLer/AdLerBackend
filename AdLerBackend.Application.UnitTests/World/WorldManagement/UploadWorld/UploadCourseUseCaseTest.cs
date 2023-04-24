using AdLerBackend.Application.Common.DTOs;
using AdLerBackend.Application.Common.DTOs.Storage;
using AdLerBackend.Application.Common.Exceptions;
using AdLerBackend.Application.Common.Interfaces;
using AdLerBackend.Application.Common.InternalUseCases.CheckUserPrivileges;
using AdLerBackend.Application.Common.Responses.LMSAdapter;
using AdLerBackend.Application.Common.Responses.World;
using AdLerBackend.Application.LMS.GetUserData;
using AdLerBackend.Application.World.WorldManagement.UploadWorld;
using AdLerBackend.Domain.Entities;
using AutoBogus;
using MediatR;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

#pragma warning disable CS8618

namespace AdLerBackend.Application.UnitTests.World.WorldManagement.UploadWorld;

public class UploadWorldUseCaseTest
{
    private IFileAccess _fileAccess;
    private ILMS _ilms;
    private ILmsBackupProcessor _lmsBackupProcessor;
    private IMediator _mediator;
    private ISerialization _serialization;
    private IWorldRepository _worldRepository;

    [SetUp]
    public void Setup()
    {
        _lmsBackupProcessor = Substitute.For<ILmsBackupProcessor>();
        _mediator = Substitute.For<IMediator>();
        _fileAccess = Substitute.For<IFileAccess>();
        _worldRepository = Substitute.For<IWorldRepository>();
        _serialization = Substitute.For<ISerialization>();
        _ilms = Substitute.For<ILMS>();

        var mockedDsl = AutoFaker.Generate<WorldDtoResponse>();
        mockedDsl.World.Elements = new List<Application.Common.Responses.World.Element>
        {
            new()
            {
                ElementId = 1,
                ElementCategory = "h5p",
                LmsElementIdentifier = new LmsElementIdentifier
                {
                    Value = "path1"
                }
            },
            new()
            {
                ElementId = 2,
                ElementCategory = "h5p",
                LmsElementIdentifier = new LmsElementIdentifier
                {
                    Value = "path2"
                }
            }
        };

        _serialization.GetObjectFromJsonStreamAsync<WorldDtoResponse>(Arg.Any<Stream>())
            .Returns(mockedDsl);
    }

    [Test]
    public async Task Handle_Valid_TriggersUpload()
    {
        // Arrange

        var systemUnderTest =
            new UploadWorldUseCase(_lmsBackupProcessor, _mediator, _fileAccess, _worldRepository,
                _serialization, _ilms);

        _mediator.Send(Arg.Any<CheckUserPrivilegesCommand>()).Returns(Unit.Task);

        _mediator.Send(Arg.Any<GetLMSUserDataCommand>()).Returns(new LMSUserDataResponse
        {
            IsAdmin = true
        });

        var fakedDsl = AutoFaker.Generate<WorldDtoResponse>();
        fakedDsl.World.Elements[0] = new Application.Common.Responses.World.Element
        {
            ElementId = 13337,
            ElementCategory = "h5p"
        };

        _lmsBackupProcessor.GetWorldDescriptionFromBackup(Arg.Any<Stream>()).Returns(fakedDsl);

        _worldRepository.ExistsForAuthor(Arg.Any<int>(), Arg.Any<string>()).Returns(false);

        _fileAccess.StoreAtfFileForWorld(Arg.Any<StoreWorldAtfDto>()).Returns("testDSlPath");

        _lmsBackupProcessor.GetH5PFilesFromBackup(Arg.Any<Stream>()).Returns(new List<H5PDto>

        {
            new()
            {
                H5PFile = new MemoryStream(),
                H5PFileName = "FileName"
            }
        });

        _fileAccess.StoreH5PFilesForWorld(Arg.Any<WorldStoreH5PDto>()).Returns(new List<string>
        {
            "path1"
        });


        // Act
        await systemUnderTest.Handle(new UploadWorldCommand
        {
            BackupFileStream = new MemoryStream(),
            ATFFileStream = new MemoryStream(),
            WebServiceToken = "testToken"
        }, CancellationToken.None);

        // Assert that AddAsync has been called with the correct entity
        await _worldRepository.Received(1)
            .AddAsync(Arg.Is<WorldEntity>(x => x.Name == fakedDsl.World.LmsElementIdentifier.Value));
    }

    [Test]
    public Task Handle_UnauthorizedUser_Throws()
    {
        // Arrange
        var systemUnderTest =
            new UploadWorldUseCase(_lmsBackupProcessor, _mediator, _fileAccess, _worldRepository,
                _serialization, _ilms);

        _mediator.Send(Arg.Any<CheckUserPrivilegesCommand>()).Throws(new ForbiddenAccessException(""));

        _mediator.Send(Arg.Any<GetLMSUserDataCommand>()).Returns(new LMSUserDataResponse
        {
            IsAdmin = false
        });

        // Act
        // Assert
        Assert.ThrowsAsync<ForbiddenAccessException>(async () =>
            await systemUnderTest.Handle(new UploadWorldCommand
            {
                BackupFileStream = new MemoryStream(),
                ATFFileStream = new MemoryStream(),
                WebServiceToken = "testToken"
            }, CancellationToken.None));
        return Task.CompletedTask;
    }

    [Test]
    public Task Handle_CourseExists_ThrowsException()
    {
        // Arrange
        var systemUnderTest =
            new UploadWorldUseCase(_lmsBackupProcessor, _mediator, _fileAccess, _worldRepository,
                _serialization, _ilms);

        _mediator.Send(Arg.Any<GetLMSUserDataCommand>()).Returns(new LMSUserDataResponse
        {
            IsAdmin = true
        });

        var fakedDsl = AutoFaker.Generate<WorldDtoResponse>();
        fakedDsl.World.Elements[0] = new Application.Common.Responses.World.Element
        {
            ElementId = 13337,
            ElementCategory = "h5p"
        };

        _lmsBackupProcessor.GetWorldDescriptionFromBackup(Arg.Any<Stream>()).Returns(fakedDsl);

        _worldRepository.ExistsForAuthor(Arg.Any<int>(), Arg.Any<string>()).Returns(true);

        // Act
        // Assert
        Assert.ThrowsAsync<WorldCreationException>(async () =>
            await systemUnderTest.Handle(new UploadWorldCommand
            {
                BackupFileStream = new MemoryStream(),
                ATFFileStream = new MemoryStream(),
                WebServiceToken = "testToken"
            }, CancellationToken.None));
        return Task.CompletedTask;
    }


    [Test]
    public async Task Handle_ValidNoH5p_TriggersUpload()
    {
        // Arrange
        var systemUnderTest =
            new UploadWorldUseCase(_lmsBackupProcessor, _mediator, _fileAccess, _worldRepository,
                _serialization, _ilms);

        _mediator.Send(Arg.Any<GetLMSUserDataCommand>()).Returns(new LMSUserDataResponse
        {
            IsAdmin = true
        });

        var fakedDsl = AutoFaker.Generate<WorldDtoResponse>();

        _lmsBackupProcessor.GetWorldDescriptionFromBackup(Arg.Any<Stream>()).Returns(fakedDsl);

        _worldRepository.ExistsForAuthor(Arg.Any<int>(), Arg.Any<string>()).Returns(false);

        _fileAccess.StoreAtfFileForWorld(Arg.Any<StoreWorldAtfDto>()).Returns("testDSlPath");

        _lmsBackupProcessor.GetH5PFilesFromBackup(Arg.Any<Stream>()).Returns(new List<H5PDto>());

        _fileAccess.StoreH5PFilesForWorld(Arg.Any<WorldStoreH5PDto>()).Returns(new List<string>
        {
            "path1"
        });


        // Act
        var result = await systemUnderTest.Handle(new UploadWorldCommand
        {
            BackupFileStream = new MemoryStream(),
            ATFFileStream = new MemoryStream(),
            WebServiceToken = "testToken"
        }, CancellationToken.None);

        // Assert that AddAsync has been called with the correct entity
        await _worldRepository.Received(1)
            .AddAsync(Arg.Is<WorldEntity>(x => x.Name == fakedDsl.World.LmsElementIdentifier.Value));
    }
}