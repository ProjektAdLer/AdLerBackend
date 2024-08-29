using AdLerBackend.Infrastructure.Repositories.Player;

namespace AdLerBackend.Infrastructure.UnitTests.Repositories.Player;

public class PlayerRepositoryTest
{
    [Test]
    // ANF-ID: [BPG22]
    public async Task Initializes()
    {
        // Arrange
        var dbContext = ContextCreator.GetNewDbContextInstance();
        // Act
        var systemUnderTest = new PlayerRepository(dbContext);

        // Assert
        Assert.That(systemUnderTest, Is.Not.Null);
    }

    [Test]
    // ANF-ID: [BPG22]
    public async Task EnsureGetAsync_ReturnsPlayer()
    {
        // Arrange
        var dbContext = ContextCreator.GetNewDbContextInstance();
        var systemUnderTest = new PlayerRepository(dbContext);

        // Act
        var result = await systemUnderTest.GetOrCreatePlayerAsync(1);

        // Assert
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    // ANF-ID: [BPG22]
    public async Task EnsureGetAsync_Existing_ReturnsPlayer()
    {
        // Arrange
        var dbContext = ContextCreator.GetNewDbContextInstance();
        var systemUnderTest = new PlayerRepository(dbContext);

        // Act
        var result = await systemUnderTest.GetOrCreatePlayerAsync(1);

        // Assert
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    // ANF-ID: [BPG22]
    public async Task EnsureGetAsync_PlayerAlreadyexisted_ExistingPlayerReturned()
    {
        // Arrange
        var dbContext = ContextCreator.GetNewDbContextInstance();
        var systemUnderTest = new PlayerRepository(dbContext);

        // Act
        var result = await systemUnderTest.GetOrCreatePlayerAsync(1);
        var result2 = await systemUnderTest.GetOrCreatePlayerAsync(1);

        Assert.Multiple(() =>
        {
            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result2, Is.Not.Null);
        });
        Assert.That(result2, Is.EqualTo(result));
    }
}