using AdLerBackend.Domain.Entities;
using AdLerBackend.Domain.UnitTests.TestingUtils;

namespace AdLerBackend.Domain.UnitTests.Entities;

public class WorldEntityTest
{
    [Test]
    public void Constructor_SetsAllParameters()
    {
        const string name = "foobar";
        var h5PFilesInCourse = new List<H5PLocationEntity>
        {
            H5PLocationEntityFactory.CreateH5PLocationEntity()
        };
        const string dslLocation = "dslLocation";
        const int authorId = 8888;
        const int id = 666;
        const int lmsId = 0;

        var world = new WorldEntity(name, h5PFilesInCourse, dslLocation, authorId, lmsId, id);

        Assert.Multiple(() =>
        {
            Assert.That(world.Name, Is.EqualTo(name));
            Assert.That(world.H5PFilesInCourse, Is.EqualTo(h5PFilesInCourse));
            Assert.That(world.DslLocation, Is.EqualTo(dslLocation));
            Assert.That(world.Id, Is.EqualTo(id));
            Assert.That(world.LmsWorldId, Is.EqualTo(lmsId));
        });
    }

    [Test]
    public void PrivateConstructor_SetsAllParameters()
    {
        var instance = TestingHelpers.GetWithPrivateConstructor<WorldEntity>();

        Assert.Multiple(() =>
        {
            Assert.That(instance.Name, Is.EqualTo(string.Empty));
            Assert.That(instance.H5PFilesInCourse, Is.EqualTo(new List<H5PLocationEntity>()));
            Assert.That(instance.DslLocation, Is.EqualTo(string.Empty));
            Assert.That(instance.Id, Is.EqualTo(null));
        });
    }

    [Test]
    public void PrivateConstructor_SetsAllParameters2()
    {
        var instance = new WorldEntity();

        Assert.Multiple(() =>
        {
            Assert.That(instance.Name, Is.EqualTo(string.Empty));
            Assert.That(instance.H5PFilesInCourse, Is.EqualTo(new List<H5PLocationEntity>()));
            Assert.That(instance.DslLocation, Is.EqualTo(string.Empty));
            Assert.That(instance.Id, Is.EqualTo(null));
        });
    }
}