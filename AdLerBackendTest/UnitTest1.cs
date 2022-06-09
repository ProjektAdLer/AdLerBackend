using NUnit.Framework;
using System.Collections;

namespace AdLerBackendTest
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            Assert.Pass();
        }

        [Test]
        [TestCaseSource(typeof(Test2Source))]
        //[TestCase(1, 2, ExpectedResult = 3)]
        //[TestCase(2, 2, ExpectedResult = 4)]
        public void Test2(int param1, int param2, int expected)
        {

            Assert.That(param1 + param2, Is.EqualTo(expected));
        }

        private class Test2Source : IEnumerable
        {
            public IEnumerator GetEnumerator()
            {
                yield return new object[] { 1, 2, 3 };
            }
        }
    }

}