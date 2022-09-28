using AdLerBackend.Infrastructure.Repositories.Player;
using AdLerBackend.Infrastructure.UnitTests.Repositories.Courses;

namespace AdLerBackend.Infrastructure.UnitTests.Repositories.Player;

public class PlayerRepositoryTest : TestWithSqlite
{
    [Test]
    public async Task Initializes()
    {
        // Arrange

        // Act
        var systemUnderTest = new PlayerRepository(DbContext);

        // Assert
        Assert.NotNull(systemUnderTest);
    }

    [Test]
    public async Task EnsureGetAsync_ReturnsPlayer()
    {
        // Arrange
        var systemUnderTest = new PlayerRepository(DbContext);

        // Act
        var result = await systemUnderTest.EnsureGetAsync(1);

        // Assert
        Assert.NotNull(result);
    }

    [Test]
    public async Task EnsureGetAsync_Existing_ReturnsPlayer()
    {
        // Arrange
        var systemUnderTest = new PlayerRepository(DbContext);

        // Act
        var result = await systemUnderTest.EnsureGetAsync(1);

        // Assert
        Assert.NotNull(result);
    }
}