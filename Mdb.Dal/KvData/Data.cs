using LiteDB;

namespace Mdb.Dal.KvData
{
    /// <summary>
    /// Represent key-value pair stored at DB.
    /// </summary>
    public class Data
    {
        /// <summary>
        /// Record ID (Key).
        /// </summary>
        [BsonId(false)]
        public string Id { get; set; }

        /// <summary>
        /// Record value.
        /// </summary>
        public string Value  { get; set; }
    }
}