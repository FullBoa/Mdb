using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Mdb.Logic.TextProcessors;

namespace Mdb.Logic.Stores
{
    /// <inheritdoc />
    public class InMemoryStore : IStore
    {
        protected readonly ConcurrentDictionary<string, string> Store;
        protected readonly ITextProcessor TextProcessor;

        public InMemoryStore(ITextProcessor textProcessor)
        {
            TextProcessor = textProcessor ?? throw new ArgumentNullException(nameof(textProcessor));
            Store = new ConcurrentDictionary<string, string>();
        }

        public InMemoryStore(IDictionary<string, string> sourceDictionary, ITextProcessor textProcessor)
        {
            if (sourceDictionary == null)
                throw new ArgumentNullException(nameof(sourceDictionary));
            Store = new ConcurrentDictionary<string, string>(sourceDictionary);

            TextProcessor = textProcessor ?? throw new ArgumentNullException(nameof(textProcessor));
        }

        /// <inheritdoc />
        public string Get(string key)
        {
            Store.TryGetValue(key, out var value);
            return value;
        }

        /// <inheritdoc />
        public void Set(string key, string value)
        {
            var processedValue = TextProcessor.Process(value);
            Store.AddOrUpdate(key, processedValue, (k, v) => processedValue);
        }

        /// <inheritdoc />
        public bool Delete(string key)
        {
            return Store.TryRemove(key, out var _);
        }

        /// <inheritdoc />
        public ICollection<string> Keys()
        {
            return Store.Keys;
        }
    }
}