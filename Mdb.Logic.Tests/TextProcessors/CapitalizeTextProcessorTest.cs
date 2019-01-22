using System.Collections;
using Mdb.Logic.TextProcessors;
using NUnit.Framework;

namespace Mdb.Logic.Tests.TextProcessors
{
    [TestFixture]
    public class CapitalizeTextProcessorTest
    {
        private static IEnumerable ProcessCases
        {
            get
            {
                yield return new TestCaseData("Hello world!")
                    .Returns("Hello world!");
                yield return new TestCaseData("The Quick Brown Fox Jumps Over the Lazy Dog.")
                    .Returns("The quick brown fox jumps over the lazy dog.");

                yield return new TestCaseData("FDFDFasdfa asdf aFafasf. blasdfk... as;safh 442 fk? 2342 whuh")
                    .Returns("Fdfdfasdfa asdf afafasf. Blasdfk... As;safh 442 fk? 2342 Whuh");
            }
        }

        [TestCaseSource(typeof(CapitalizeTextProcessorTest), nameof(ProcessCases))]
        public string Process(string text)
        {
            var processor = GetProcessor();
            return processor.Process(text);
        }

        private CapitalizeTextProcessor GetProcessor()
        {
            var processor = new CapitalizeTextProcessor();
            return processor;
        }
    }
}