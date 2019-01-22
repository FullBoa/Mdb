using System.Collections.Generic;

namespace Mdb.Logic.TextProcessors
{
    /// <inheritdoc />
    public class CapitalizeTextProcessor : ITextProcessor
    {
        private readonly ICollection<char> _separators = new List<char>{'.', '!', '?'};

        /// <inheritdoc />
        public string Process(string text)
        {
            if (string.IsNullOrEmpty(text))
                return text;

            text = text.ToLower();
            var chars = text.ToCharArray();
            var needCapitalized = true;
            for (var i = 0; i < chars.Length; i++)
            {
                if (needCapitalized && char.IsLetter(chars[i]))
                {
                    chars[i] = char.ToUpper(chars[i]);
                    needCapitalized = false;
                    continue;
                }

                if (_separators.Contains(chars[i]))
                    needCapitalized = true;
            }

            return new string(chars);
        }
    }
}