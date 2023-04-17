using AdLerBackend.Infrastructure.Repositories.Player;

namespace AdLerBackend.Infrastructure.UnitTests.Repositories.Player;

public class PlayerRepositoryTest
{
    [Test]
    public async Task Initializes()
    {
        // Arrange
        var dbContext = ContextCreator.GetNewDbContextInstance();
        // Act
        var systemUnderTest = new PlayerRepository(dbContext);

        // Assert
        Assert.NotNull(systemUnderTest);
    }

    [Test]
    public async Task EnsureGetAsync_ReturnsPlayer()
    {
        // Arrange
        var dbContext = ContextCreator.GetNewDbContextInstance();
        var systemUnderTest = new PlayerRepository(dbContext);

        // Act
        var result = await systemUnderTest.EnsureGetAsync(1);

        // Assert
        Assert.NotNull(result);
    }

    [Test]
    public async Task EnsureGetAsync_Existing_ReturnsPlayer()
    {
        // Arrange
        var dbContext = ContextCreator.GetNewDbContextInstance();
        var systemUnderTest = new PlayerRepository(dbContext);

        // Act
        var result = await systemUnderTest.EnsureGetAsync(1);

        // Assert
        Assert.NotNull(result);
    }

    [Test]
    public async Task EnsureGetAsync_PlayerAlreadyexisted_ExistingPlayerReturned()
    {
        // Arrange
        var dbContext = ContextCreator.GetNewDbContextInstance();
        var systemUnderTest = new PlayerRepository(dbContext);

        // Act
        var result = await systemUnderTest.EnsureGetAsync(1);
        var result2 = await systemUnderTest.EnsureGetAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result2);
        Assert.AreEqual(result, result2);
    }
}