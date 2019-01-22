namespace Mdb.Logic.TextProcessors
{
    /// <summary>
    /// Makes text transformation.
    /// </summary>
    public interface ITextProcessor
    {
        /// <summary>
        /// Transforms text.
        /// </summary>
        /// <param name="text">Original text.</param>
        /// <returns>Transformed text.</returns>
        string Process(string text);
    }
}