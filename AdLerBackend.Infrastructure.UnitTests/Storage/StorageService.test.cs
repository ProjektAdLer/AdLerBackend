using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using AdLerBackend.Application.Common.DTOs;
using AdLerBackend.Application.Common.DTOs.Storage;
using AdLerBackend.Application.Common.Responses.World;
using AdLerBackend.Infrastructure.Storage;
using AutoBogus;

#pragma warning disable CS8618

namespace AdLerBackend.Infrastructure.UnitTests.Storage;

public class StorageServiceTest
{
    private FileStream _backupFileStream;
    private IFileSystem _fileSystem;

    [OneTimeSetUp]
    public void OneTimeSetup()
    {
        _backupFileStream = new FileStream("../../../Storage/TestFiles/FakeH5p.zip", FileMode.Open);
    }

    [SetUp]
    public void Setup()
    {
        _fileSystem = new MockFileSystem();
    }

    [Test]
    public void StoreH5PFilesForCourse_Valid_StoresFiles()
    {
        // Arrange
        var storageService = new StorageService(_fileSystem);
        var courseDtoFake = new WorldStoreH5PDto
        {
            AuthorId = 1,
            WorldInformation = AutoFaker.Generate<WorldAtfResponse>(),
            H5PFiles = new List<H5PDto>
            {
                new()
                {
                    H5PFile = _backupFileStream,
                    H5PFileName = "H5PName"
                }
            }
        };

        courseDtoFake.WorldInformation.World.WorldName = "LearningWorldIdentifier";

        // Act
        var returnedValue = storageService.StoreH5PFilesForWorld(courseDtoFake);

        // Assert
        Assert.That(returnedValue!.First().Key, Is.EqualTo("H5PName"));
        Assert.IsTrue(_fileSystem.File.Exists(Path.Combine("wwwroot", "courses", "1", "LearningWorldIdentifier",
            "h5p", "H5PName.h5p")));
    }


    [Test]
    public void DeleteCourse_Valid_DeletesCourse()
    {
        // Arrange
        var dto = new WorldDeleteDto
        {
            AuthorId = 1,
            WorldName = "CourseName"
        };
        var folder = Path.Combine("wwwroot", "courses", "1", "CourseName");
        var file = Path.Combine(folder, "CourseName.json");

        _fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
        {
            {file, new MockFileData("Testing is meh.")}
        });
        var storageService = new StorageService(_fileSystem);

        // Act
        storageService.DeleteWorld(dto);

        // Assert
        Assert.IsFalse(_fileSystem.Directory.Exists(folder));
    }
}