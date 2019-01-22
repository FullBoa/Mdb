using System.Collections;
using System.Collections.Generic;
using FluentAssertions;
using Mdb.Logic.Stores;
using Mdb.Logic.TextProcessors;
using Moq;
using NUnit.Framework;

namespace Mdb.Logic.Tests.Stores
{
    [TestFixture]
    public class InMemoryStoreTest
    {
        private ITextProcessor _textProcessorMock;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            var mock = new Mock<ITextProcessor>();
            mock.Setup(p => p.Process(It.IsAny<string>()))
                .Returns((string v) => v);
            _textProcessorMock = mock.Object;
        }


        [TestCase("key", ExpectedResult = "value")]
        [TestCase("sql", ExpectedResult = "postgres")]
        [TestCase("nosql", ExpectedResult = "mongo")]
        [TestCase("kvdb", ExpectedResult = "ssdb")]
        [TestCase("language", ExpectedResult = "C#")]
        [TestCase("key2", ExpectedResult = null)]
        [TestCase("newsql", ExpectedResult = null)]
        [TestCase("secondlanguage", ExpectedResult = null)]
        public string Get(string key)
        {
            var store = GetStore();

            return store.Get(key);
        }

        private static IEnumerable SetCases
        {
            get
            {
                yield return new TestCaseData("key", "newvalue").Returns(("value", "newvalue"));
                yield return new TestCaseData("sql", "mssql server").Returns(("postgres", "mssql server"));
                yield return new TestCaseData("nosql", "aerospike").Returns(("mongo", "aerospike"));
                yield return new TestCaseData("kvdb", "redis").Returns(("ssdb", "redis"));
                yield return new TestCaseData("language", "golang").Returns(("C#", "golang"));
                yield return new TestCaseData("cloud", "amazon").Returns(((string) null, "amazon"));
                yield return new TestCaseData("ide", "rider").Returns(((string) null, "rider"));
            }
        }

        [TestCaseSource(typeof(InMemoryStoreTest), nameof(SetCases))]
        public (string, string) Set(string key, string value)
        {
            var store = GetStore();

            var oldValue = store.Get(key);
            store.Set(key, value);
            var newValue = store.Get(key);

            return (oldValue, newValue);
        }

        private static IEnumerable DeleteCases
        {
            get
            {
                yield return new TestCaseData("key").Returns(("value", true, (string) null));
                yield return new TestCaseData("sql").Returns(("postgres", true, (string) null));
                yield return new TestCaseData("nosql").Returns(("mongo", true, (string) null));
                yield return new TestCaseData("kvdb").Returns(("ssdb", true, (string) null));
                yield return new TestCaseData("language").Returns(("C#", true, (string) null));
                yield return new TestCaseData("key2").Returns(((string) null, false, (string) null));
                yield return new TestCaseData("newsql").Returns(((string) null, false, (string) null));
                yield return new TestCaseData("secondlanguage").Returns(((string) null, false, (string) null));
            }
        }

        [TestCaseSource(typeof(InMemoryStoreTest), nameof(DeleteCases))]
        public (string, bool, string) Delete(string key)
        {
            var store = GetStore();

            var oldValue = store.Get(key);
            var ok = store.Delete(key);
            var newValue = store.Get(key);

            return (oldValue, ok, newValue);
        }

        private static IEnumerable KeysCases
        {
            get
            {
                var data = new Dictionary<string, string>
                {
                    { "key", "value" },
                    { "sql", "postgres" },
                    { "nosql", "mongo" },
                    { "kvdb", "ssdb" },
                    { "language", "C#" }
                };
                ICollection expected = new List<string> { "key", "sql", "nosql", "kvdb", "language" };
                yield return new TestCaseData(data, expected);

                data = new Dictionary<string, string>();
                expected = new List<string>();
                yield return new TestCaseData(data, expected);
            }
        }

        [TestCaseSource(typeof(InMemoryStoreTest), nameof(KeysCases))]
        public void Keys(
            IDictionary<string, string> originData,
            ICollection<string> expectedKeys)
        {
            var store = new InMemoryStore(originData, _textProcessorMock);
            var keys = store.Keys();
            keys.Should().BeEquivalentTo(expectedKeys);
        }

        private InMemoryStore GetStore()
        {
            var originData = new Dictionary<string, string>
            {
                { "key", "value" },
                { "sql", "postgres" },
                { "nosql", "mongo" },
                { "kvdb", "ssdb" },
                { "language", "C#" }
            };
            return new InMemoryStore(originData, _textProcessorMock);
        }
    }
}