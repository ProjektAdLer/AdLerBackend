﻿using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using AdLerBackend.Application.Common.DTOs;
using AdLerBackend.Application.Common.DTOs.Storage;
using AdLerBackend.Application.Common.Responses.World;
using AdLerBackend.Infrastructure.Storage;
using AutoBogus;
using Microsoft.Extensions.Logging;
using NSubstitute;

#pragma warning disable CS8618

namespace AdLerBackend.Infrastructure.UnitTests.Storage;

public class StorageServiceTest
{
    private FileStream _backupFileStream;
    private IFileSystem _fileSystem;
    private ILogger<StorageService> _mockedLogger;

    [OneTimeTearDown]
    public void OneTimeTearDown()
    {
        _backupFileStream?.Close();
    }

    [OneTimeSetUp]
    public void OneTimeSetup()
    {
        _backupFileStream = new FileStream("../../../Storage/TestFiles/FakeH5p.zip", FileMode.Open);
    }

    [SetUp]
    public void Setup()
    {
        _fileSystem = new MockFileSystem();
        _mockedLogger = Substitute.For<ILogger<StorageService>>();
    }

    [Test]
    public void StoreH5PFilesForCourse_Valid_StoresFiles()
    {
        // Arrange
        var storageService = new StorageService(_fileSystem, _mockedLogger);
        var courseDtoFake = new WorldStoreH5PDto
        {
            CourseInstanceId = 1,
            WorldInformation = AutoFaker.Generate<WorldAtfResponse>(),
            H5PFiles = new List<H5PDto>
            {
                new()
                {
                    H5PFile = _backupFileStream,
                    H5PUuid = "H5PUUID"
                }
            }
        };

        // Act
        var returnedValue = storageService.StoreH5PFilesForWorld(courseDtoFake);

        Assert.Multiple(() =>
        {
            // Assert
            Assert.That(returnedValue!.First().Key, Is.EqualTo("H5PUUID"));
            Assert.That(_fileSystem.File.Exists(Path.Combine("wwwroot", "courses", "1",
                "h5p", "H5PUUID.h5p")), Is.True);
        });
    }


    [Test]
    public void DeleteCourse_Valid_DeletesCourse()
    {
        // Arrange
        var dto = new WorldDeleteDto
        {
            WorldInstanceId = 1337
        };
        var folder = Path.Combine("wwwroot", "courses", "1337");
        var file = Path.Combine(folder, "CourseName.json");

        _fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
        {
            {file, new MockFileData("Testing is meh.")}
        });
        var storageService = new StorageService(_fileSystem, _mockedLogger);

        // Act
        storageService.DeleteWorld(dto);

        // Assert
        Assert.That(_fileSystem.Directory.Exists(folder), Is.False);
    }

    [Test]
    public void DeleteWorld_NonexistentDirectory_ReturnsTrue()
    {
        // Arrange
        var dto = new WorldDeleteDto
        {
            WorldInstanceId = 9999 // An ID that doesn't exist
        };
        _fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>()); // Empty file system
        var storageService = new StorageService(_fileSystem, _mockedLogger);

        // Act
        var result = storageService.DeleteWorld(dto);

        // Assert
        Assert.That(result, Is.True);
        Assert.That(_fileSystem.Directory.Exists(Path.Combine("wwwroot", "courses", "9999")), Is.False);
    }

    [Test]
    public void DeleteWorld_NonExistentDirectory_LogsWarning()
    {
        // Arrange
        var dto = new WorldDeleteDto
        {
            WorldInstanceId = 9999 // An ID that doesn't exist
        };
        _fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>()); // Empty file system
        var storageService = new StorageService(_fileSystem, _mockedLogger);

        // Act
        storageService.DeleteWorld(dto);

        // Assert
        _mockedLogger.Received(1).LogWarning("World with ID 9999 does not exist.");
    }
}