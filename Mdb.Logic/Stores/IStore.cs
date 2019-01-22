using System.Collections.Generic;

namespace Mdb.Logic.Stores
{
    /// <summary>
    /// Store for key-value pair
    /// </summary>
    public interface IStore
    {
        /// <summary>
        /// Gets value by its key.
        /// </summary>
        /// <param name="key">Key of pair.</param>
        /// <returns>Value by its key.</returns>
        string Get(string key);

        /// <summary>
        /// Sets value to key.
        /// </summary>
        /// <param name="key">Key of pair.</param>
        /// <param name="value">Value of key.</param>
        void Set(string key, string value);

        /// <summary>
        /// Deletes key-value pair.
        /// </summary>
        /// <param name="key">Key of pair.</param>
        /// <returns>True if key exists.</returns>
        bool Delete(string key);

        /// <summary>
        /// Returns all keys existed in store
        /// </summary>
        /// <returns><see cref="ICollection{T}"/> of keys</returns>
        ICollection<string> Keys();
    }
}