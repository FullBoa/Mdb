using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using LiteDB;
using Mdb.Dal;
using Mdb.Dal.KvData;
using Mdb.Logic.Stores;
using Mdb.Logic.TextProcessors;
using Moq;
using NUnit.Framework;

namespace Mdb.Logic.Tests.Stores
{
    [Category("integration")]
    [NonParallelizable]
    public class DataRepositoryTest
    {
        private const string DataFilename = "/app/data/data.db";
        private const string DataCollectionName = "Data";

        private ITextProcessor _textProcessor;
        private DataRepository _repository;
        private LiteDatabase _db;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            var mock = new Mock<ITextProcessor>();
            mock.Setup(p => p.Process(It.IsAny<string>()))
                .Returns((string v) => v);
            _textProcessor = mock.Object;

            var connectionString = new ConnectionString
            {
                Filename = DataFilename,
            };
            _db = new LiteDatabase(connectionString, BsonMapper.Global);

            _repository = new DataRepository(DataFilename);
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            _db.Dispose();
            _repository.Dispose();
        }

        [SetUp]
        public void Setup()
        {
            var collections = _db.GetCollectionNames().ToList();
            foreach (var collection in collections)
            {
                _db.DropCollection(collection);
            }

            var data = new List<BsonDocument>
            {
                new BsonDocument { ["_id"] = "language", ["Value"] = "C#" },
                new BsonDocument { ["_id"] = "cloud", ["Value"] = "Amazon" },
                new BsonDocument { ["_id"] = "sql", ["Value"] = "Postgres" }
            };

            var dataCollection = _db.GetCollection(DataCollectionName);
            dataCollection.InsertBulk(data);
            dataCollection.EnsureIndex("_id", true);
        }

        [TestCase("language", ExpectedResult = "C#")]
        [TestCase("cloud", ExpectedResult = "Amazon")]
        [TestCase("sql", ExpectedResult = "Postgres")]
        [TestCase("nosql", ExpectedResult = (string) null)]
        [TestCase("kbdb", ExpectedResult = (string) null)]
        public string Get(string key)
        {
            var store = GetStore();
            return store.Get(key);
        }

        [Test]
        public void Set()
        {
            var store = GetStore();

            store.Set("nosql", "MongoDB");
            store.Set("kvdb", "Aerospike");
            store.Set("language", "Golang");
            store.Set("cloud", "Azure");

            var expectedData = new List<BsonDocument>
            {
                new BsonDocument { ["_id"] = "language", ["Value"] = "Golang" },
                new BsonDocument { ["_id"] = "cloud", ["Value"] = "Azure" },
                new BsonDocument { ["_id"] = "nosql", ["Value"] = "MongoDB" },
                new BsonDocument { ["_id"] = "kvdb", ["Value"] = "Aerospike" },
                new BsonDocument { ["_id"] = "sql", ["Value"] = "Postgres" }
            };
            var actualData = _db.GetCollection(DataCollectionName).FindAll().ToList();
            actualData.Should().BeEquivalentTo(expectedData);
        }

        [Test]
        public void Delete()
        {
            var store = GetStore();

            store.Delete("language");
            store.Delete("cloud");
            var expectedData = new List<BsonDocument>
            {
                new BsonDocument { ["_id"] = "sql", ["Value"] = "Postgres" }
            };
            var actualData = _db.GetCollection(DataCollectionName).FindAll().ToList();
            actualData.Should().BeEquivalentTo(expectedData);
        }

        [Test]
        public void Keys()
        {
            var store = GetStore();

            var expectedData = new List<string> { "language", "cloud", "sql" };
            var actualData = store.Keys();
            actualData.Should().BeEquivalentTo(expectedData);
        }

        private PersistentStore GetStore()
        {
            var store = new PersistentStore(_repository, _textProcessor);
            return store;
        }
    }
}