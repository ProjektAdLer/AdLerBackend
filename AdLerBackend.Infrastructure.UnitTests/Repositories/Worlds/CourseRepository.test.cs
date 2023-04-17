using AdLerBackend.Domain.UnitTests.TestingUtils;
using AdLerBackend.Infrastructure.Repositories.Worlds;

namespace AdLerBackend.Infrastructure.UnitTests.Repositories.Worlds;

public class WorldRepositoryTest
{
    [Test]
    public async Task GetAllCoursesForAuthor_Valid_GetsThen()
    {
        // Arrange
        var dbContext = ContextCreator.GetNewDbContextInstance();
        var systemUnderTest = new WorldRepository(dbContext);

        var world1 = WorldEntityFactory.CreateWorldEntity(authorId: 2, id: 1);

        await systemUnderTest.AddAsync(world1);

        world1.Id = 2;

        await systemUnderTest.AddAsync(world1);

        // Act
        var result = await systemUnderTest.GetAllForAuthor(2);

        // Assert
        Assert.That(result, Has.Count.EqualTo(2));
    }

    [Test]
    public async Task ExistsCourseForAuthor_Valid_GetsResult()
    {
        // Arrange
        var dbContext = ContextCreator.GetNewDbContextInstance();
        var systemUnderTest = new WorldRepository(dbContext);

        await systemUnderTest.AddAsync(WorldEntityFactory.CreateWorldEntity(authorId: 2, name: "Test Course"));

        // Act
        var result = await systemUnderTest.ExistsForAuthor(2, "Test Course");

        // Assert
        Assert.That(result, Is.True);

        // Act
        var result2 = await systemUnderTest.ExistsForAuthor(2, "Test CourseFOO");

        // Assert
        Assert.That(result2, Is.False);
    }

    [Test]
    public async Task GetAllCoursesByStrings_Valid_GetsResult()
    {
        // Arrange
        var dbContext = ContextCreator.GetNewDbContextInstance();
        var systemUnderTest = new WorldRepository(dbContext);

        await systemUnderTest.AddAsync(WorldEntityFactory.CreateWorldEntity("Test Course"));

        await systemUnderTest.AddAsync(WorldEntityFactory.CreateWorldEntity("Test Course 2"));

        // Act
        var result = await systemUnderTest.GetAllByStrings(new List<string> {"Test Course", "Test Course 2"});

        // Assert
        Assert.That(result, Has.Count.EqualTo(2));
    }

    [Test]
    public async Task GetAsync_Valid_GetsResult()
    {
        // Arrange
        var dbContext = ContextCreator.GetNewDbContextInstance();
        var systemUnderTest = new WorldRepository(dbContext);

        await systemUnderTest.AddAsync(WorldEntityFactory.CreateWorldEntity(id: 1));

        // Act
        var result = await systemUnderTest.GetAsync(1);

        // Assert
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public async Task Delete_Valid_DeletesCourse_DeletesCourse()
    {
        // Arrange
        var dbContext = ContextCreator.GetNewDbContextInstance();
        var systemUnderTest = new WorldRepository(dbContext);

        await systemUnderTest.AddAsync(WorldEntityFactory.CreateWorldEntity(id: 1));

        // Act
        await systemUnderTest.DeleteAsync(1);

        // Assert
        var allCourses = await systemUnderTest.GetAllAsync();
        Assert.That(allCourses, Has.Count.EqualTo(0));
    }
}