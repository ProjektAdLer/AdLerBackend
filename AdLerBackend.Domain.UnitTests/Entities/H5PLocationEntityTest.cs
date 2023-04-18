using AdLerBackend.Domain.Entities;
using AdLerBackend.Domain.UnitTests.TestingUtils;

namespace AdLerBackend.Domain.UnitTests.Entities;

public class H5PLocationEntityTest
{
    [Test]
    public void Constructor_SetsAllParameters()
    {
        const string path = "foobar";
        const int elementId = 8888;
        const int id = 666;

        var h5PLocationEntity = new H5PLocationEntity(path, elementId, id);

        Assert.Multiple(() =>
        {
            Assert.That(h5PLocationEntity.Path, Is.EqualTo(path));
            Assert.That(h5PLocationEntity.ElementId, Is.EqualTo(elementId));
            Assert.That(h5PLocationEntity.Id, Is.EqualTo(id));
        });
    }

    // Test private constructor
    [Test]
    public void PrivateConstructor_SetsAllParameters()
    {
        var instance = TestingHelpers.GetWithPrivateConstructor<H5PLocationEntity>();

        Assert.Multiple(() =>
        {
            Assert.That(instance.Path, Is.EqualTo(string.Empty));
            Assert.That(instance.ElementId, Is.EqualTo(0));
            Assert.That(instance.Id, Is.EqualTo(null));
        });
    }
}