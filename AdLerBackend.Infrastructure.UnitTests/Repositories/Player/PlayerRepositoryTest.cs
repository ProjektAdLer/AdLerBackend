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
}