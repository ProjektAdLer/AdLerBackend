﻿using AdLerBackend.Domain.Entities;
using AdLerBackend.Domain.UnitTests.TestingUtils;
using AdLerBackend.Infrastructure.Repositories.Common;

namespace AdLerBackend.Infrastructure.UnitTests.Repositories.Common;

public class GenericRepositoryTest
{
    [Test]
    // ANF-ID: [BPG10, BPG9, BPG8]
    public async Task Add_Valid_AddsAEntityToDB()
    {
        // Arrange
        var dbContext = ContextCreator.GetNewDbContextInstance();
        var testEntity = WorldEntityFactory.CreateWorldEntity();
        var repository = new GenericRepository<WorldEntity, int>(dbContext);

        // Act
        await repository.AddAsync(testEntity);

        // Assert, that the entity was added to the database
        var entity = dbContext.Worlds.FirstOrDefault();
        Assert.That(entity, Is.Not.Null);
    }

    [Test]
    public async Task Delete_Valid_DeletesEntityFromDb()
    {
        // Arrange
        var dbContext = ContextCreator.GetNewDbContextInstance();
        var testEntity = WorldEntityFactory.CreateWorldEntity();

        var repository = new GenericRepository<WorldEntity, int>(dbContext);
        await repository.AddAsync(testEntity);

        // Act
        await repository.DeleteAsync(1);

        // Assert, that the entity was deleted from the database
        var entity = dbContext.Worlds.FirstOrDefault();
        Assert.That(entity, Is.Null);
    }

    [Test]
    public async Task Exists_Valid_ReturnsTrueIfEntitEsists()
    {
        // Arrange
        var dbContext = ContextCreator.GetNewDbContextInstance();
        var testEntity =
            WorldEntityFactory.CreateWorldEntity(id: 1);

        var repository = new GenericRepository<WorldEntity, int>(dbContext);

        // Act
        await repository.AddAsync(testEntity);

        var exists = await repository.Exists(1);

        // Assert
        Assert.That(exists, Is.True);
    }

    [Test]
    public async Task Exists_Valid_ReturnsFalseIfEntitDoesNotEsists()
    {
        // Arrange
        var dbContext = ContextCreator.GetNewDbContextInstance();
        var repository = new GenericRepository<WorldEntity, int>(dbContext);

        // Act

        var exists = await repository.Exists(1);

        // Assert
        Assert.That(exists, Is.False);
    }

    [Test]
    public async Task GetAll_Valid_GetsAllEntites()
    {
        // Arrange
        var dbContext = ContextCreator.GetNewDbContextInstance();
        var testEntity =
            WorldEntityFactory.CreateWorldEntity("Test World", null, 1, "test", 0, 1);
        var repository = new GenericRepository<WorldEntity, int>(dbContext);

        // Act
        await repository.AddAsync(testEntity);

        testEntity = WorldEntityFactory.CreateWorldEntity("Test World", null, 1, "test", 0, 2);

        await repository.AddAsync(testEntity);

        var allEntities = await repository.GetAllAsync();

        // Assert
        Assert.That(allEntities, Has.Count.EqualTo(2));
    }

    [Test]
    public async Task Update_Valid_UpdatesEntity()
    {
        // Arrange
        var dbContext = ContextCreator.GetNewDbContextInstance();
        var testEntity = WorldEntityFactory.CreateWorldEntity("Test World", null, 1, "test", 0, 1);

        var repository = new GenericRepository<WorldEntity, int>(dbContext);

        await repository.AddAsync(testEntity);

        // Act
        testEntity.Name = "Updated Name";
        await repository.UpdateAsync(testEntity);

        // Assert
        var entity = dbContext.Worlds.FirstOrDefault();
        Assert.That(entity.Name, Is.EqualTo("Updated Name"));
    }
}