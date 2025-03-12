using System.Reflection;

namespace AdLerBackend.Domain.UnitTests.TestingUtils;

public static class TestingHelpers
{
    public static T GetWithPrivateConstructor<T>()
    {
        var privateConstructor = typeof(T).GetConstructors(BindingFlags.NonPublic | BindingFlags.Instance)[0];
        return (T) privateConstructor.Invoke(new object[] { });
    }
}