using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using AdLerBackend.Application.Common.DTOs;
using AdLerBackend.Application.Common.DTOs.Storage;
using AdLerBackend.Application.Common.Exceptions;
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
        var CourseDtoFake = new CourseStoreH5PDto
        {
            AuthorId = 1,
            CourseInforamtion = AutoFaker.Generate<DslFileDto>(),
            H5PFiles = new List<H5PDto>
            {
                new()
                {
                    H5PFile = _backupFileStream,
                    H5PFileName = "H5PName"
                }
            }
        };

        CourseDtoFake.CourseInforamtion.LearningWorld.Identifier.Value = "LearningWorldIdentifier";

        // Act
        var retunValue = storageService.StoreH5PFilesForCourse(CourseDtoFake);

        // Assert
        var path = _fileSystem.Path.Combine("wwwroot","courses","1","LearningWorldIdentifier","h5p","H5PName","Folder");
        var fileInFolder =
            _fileSystem.Path.Combine("wwwroot","courses","1","LearningWorldIdentifier","h5p","H5PName","Folder","FileInFolder");
        var textFile = _fileSystem.Path.Combine("wwwroot","courses","1","LearningWorldIdentifier","h5p","H5PName","fileAtRoot.txt");
        Assert.IsTrue(_fileSystem.Directory.Exists(path));
        Assert.IsTrue(
            _fileSystem.File.Exists(fileInFolder));
        Assert.IsTrue(_fileSystem.File.Exists(textFile));


        Assert.That(retunValue!, Has.Count.EqualTo(1));
        Assert.That(retunValue![0], Is.EqualTo(_fileSystem.Path.Combine("wwwroot","courses","1","LearningWorldIdentifier","h5p","H5PName")));
    }

    [Test]
    public void StoreDslFileForCourse_Valid_StoresFile()
    {
        // Arrange
        var storageService = new StorageService(_fileSystem);
        var dto = new StoreCourseDslDto
        {
            AuthorId = 1,
            CourseInforamtion = AutoFaker.Generate<DslFileDto>(),
            //DSL_Document.json contains data that is required for the test and should be loaded from disk
            DslFile = new FileStream("../../../Storage/TestFiles/DSL_Document.json", FileMode.Open)
        };

        dto.CourseInforamtion.LearningWorld.Identifier.Value = "LearningWorldIdentifier";

        // Act
        var dslLocation = storageService.StoreDslFileForCourse(dto);

        // Assert
        var file = _fileSystem.Path.Combine("wwwroot", "courses", "1", "LearningWorldIdentifier",
            "LearningWorldIdentifier.json");
        Assert.IsTrue(_fileSystem.File.Exists(file));


        Assert.That(dslLocation, Is.EqualTo(file));
    }

    [Test]
    public void GetFileStream_Valid_ShoudGetFileStream()
    {
        // Arrange
        var filePath = @"c:\myfile.txt";

        _fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
        {
            {@"c:\myfile.txt", new MockFileData("Testing is meh.")}
        });
        var storageService = new StorageService(_fileSystem);

        // Act
        var fileStream = storageService.GetFileStream(filePath);

        // Assert
        Assert.That(fileStream, Is.Not.Null);
    }

    [Test]
    public void GetFileStream_Invalid_ShoudThrowException()
    {
        // Arrange
        var filePath = @"c:\myfile.txt";

        var storageService = new StorageService(_fileSystem);

        // Act
        // Assert
        Assert.Throws<NotFoundException>(() => storageService.GetFileStream(filePath));
    }

    [Test]
    public void StoreH5PBase_Valid_StoresH5pBase()
    {
        // Arrange
        var storageService = new StorageService(_fileSystem);

        var h5pStream = _backupFileStream;

        // Act
        var h5pBaseLocation = storageService.StoreH5PBase(h5pStream);

        // Assert
        var path = _fileSystem.Path.Combine("wwwroot", "common", "h5pBase");
        var file = _fileSystem.Path.Combine(path, "fileAtRoot.txt");
        Assert.IsTrue(_fileSystem.Directory.Exists(path));
        Assert.IsTrue(_fileSystem.File.Exists(file));

        Assert.That(h5pBaseLocation, Is.EqualTo(path));
    }

    [Test]
    public void DeleteCourse_Valid_DeletesCourse()
    {
        // Arrange
        var dto = new CourseDeleteDto
        {
            AuthorId = 1,
            CourseName = "CourseName"
        };
        var folder = Path.Combine("wwwroot", "courses", "1", "CourseName");
        var file = Path.Combine(folder, "CourseName.json");

        _fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
        {
            {file, new MockFileData("Testing is meh.")}
        });
        var storageService = new StorageService(_fileSystem);

        // Act
        storageService.DeleteCourse(dto);

        // Assert
        Assert.IsFalse(_fileSystem.Directory.Exists(folder));
    }
}