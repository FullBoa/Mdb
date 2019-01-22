using System.Collections.Generic;
using System.Linq;
using Mdb.Dal;
using Mdb.Dal.KvData;
using Mdb.Logic.TextProcessors;

namespace Mdb.Logic.Stores
{
    /// <inheritdoc />
    public class PersistentStore : IStore
    {
        protected readonly IDataRepository Repository;
        protected readonly ITextProcessor TextProcessor;

        public PersistentStore(IDataRepository repository, ITextProcessor textProcessor)
        {
            Repository = repository;
            TextProcessor = textProcessor;
        }

        /// <inheritdoc />
        public string Get(string key)
        {
            return Repository.GetById(key);
        }

        /// <inheritdoc />
        public void Set(string key, string value)
        {
            var processedValue = TextProcessor.Process(value);
            Repository.Set(key, processedValue);
        }

        /// <inheritdoc />
        public bool Delete(string key)
        {
            return Repository.Delete(key);
        }

        /// <inheritdoc />
        public ICollection<string> Keys()
        {
            return Repository.GetAll().Select(d => d.Id).ToList();
        }
    }
}