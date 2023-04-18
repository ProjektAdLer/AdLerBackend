using AdLerBackend.Domain.Entities.PlayerData;
using AdLerBackend.Domain.UnitTests.TestingUtils;

namespace AdLerBackend.Domain.UnitTests.Entities;

public class PlayerDataEntityTest
{
    [Test]
    public void Constructor_SetsAllParameters()
    {
        var gender = PlayerAvatarGender.Male;
        var color = PlayerWorldColor.Blue;
        var id = 666;

        var playerDataEntity = new PlayerData(gender, color, id);


        Assert.Multiple(() =>
        {
            Assert.That(playerDataEntity.PlayerWorldColor, Is.EqualTo(color));
            Assert.That(playerDataEntity.PlayerGender, Is.EqualTo(gender));
            Assert.That(playerDataEntity.Id, Is.EqualTo(id));
        });
    }

    [Test]
    public void PrivateConstructor_SetsAllParameters()
    {
        var instance = TestingHelpers.GetWithPrivateConstructor<PlayerData>();

        Assert.Multiple(() =>
        {
            Assert.That(instance.PlayerWorldColor, Is.EqualTo(PlayerWorldColor.Blue));
            Assert.That(instance.PlayerGender, Is.EqualTo(PlayerAvatarGender.Male));
            Assert.That(instance.Id, Is.EqualTo(null));
        });
    }
}