using System.Collections.Generic;

namespace Mdb.Dal.KvData
{
    /// <summary>
    /// Repository for operations with data in LiteDB store.
    /// </summary>
    public interface IDataRepository
    {
        /// <summary>
        /// Gets all records from DB.
        /// </summary>
        /// <returns>Enumerable of <see cref="Data"/></returns>
        IEnumerable<Data> GetAll();

        /// <summary>
        /// Gets record by its ID.
        /// </summary>
        /// <param name="key">Key (ID) of record.</param>
        /// <returns>Value of pair stored in <see cref="Data"/></returns>
        string GetById(string key);

        /// <summary>
        /// Sets key-value pair to DB.
        /// </summary>
        /// <param name="key">Key of pair (ID).</param>
        /// <param name="value">Pair value.</param>
        void Set(string key, string value);

        /// <summary>
        /// Deletes pair from DB by ID (key).
        /// </summary>
        /// <param name="key">Key (ID) of record.</param>
        /// <returns>true if record was deleted false otherwise.</returns>
        bool Delete(string key);
    }
}