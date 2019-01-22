using System;
using System.Collections.Generic;
using LiteDB;

namespace Mdb.Dal.KvData
{
    /// <inheritdoc cref="IDataRepository" />
    public class DataRepository : IDataRepository, IDisposable
    {
        private const string DatafileConfigPath = "DataFilename";

        private readonly LiteDatabase _db;

        public DataRepository(string databaseFilename)
        {
            var connectionString = new ConnectionString
            {
                Filename = databaseFilename,
            };

            _db = new LiteDatabase(connectionString, BsonMapper.Global);
        }

        /// <inheritdoc />
        public IEnumerable<Data> GetAll() => _db.GetCollection<Data>().FindAll();


        /// <inheritdoc />
        public string GetById(string key)
        {
            var pair = _db.GetCollection<Data>().FindById(key);
            return pair?.Value;
        }

        /// <inheritdoc />
        public void Set(string key, string value)
        {
            var dbPair = new Data
            {
                Id = key,
                Value = value
            };
            _db.GetCollection<Data>().Upsert(dbPair);
            _db.GetCollection<Data>().EnsureIndex(data => data.Id, true);
        }

        /// <inheritdoc />
        public bool Delete(string key) => _db.GetCollection<Data>().Delete(key);

        public void Dispose()
        {
            _db?.Dispose();
        }
    }
}