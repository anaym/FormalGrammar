using FluentAssertions;
using FormalGrammar.Model;
using FormalGrammar.Utils;
using NUnit.Framework;

namespace FormalGrammar.Tests
{
    [TestFixture]
    public class GrammarParser_Should
    {
        [Test]
        public static void CorrectParse_WeakPrecedenceGrammar()
        {
            var rules = new[]
            {
                "S aASb",
                "S d",
                "A Ac",
                "A c",
            };

            var grammar = GrammarParser.Parse(rules);

            var expectedSRules = new []
            {
                new Rule(new NonTerminal('S'), new Terminal('a'), new NonTerminal('A'), new NonTerminal('S'), new Terminal('b')),
                new Rule(new NonTerminal('S'), new Terminal('d')),
            };

            var expectedARules = new[]
            {
                new Rule(new NonTerminal('A'), new NonTerminal('A'), new Terminal('c')),
                new Rule(new NonTerminal('A'), new Terminal('c')),
            };

            grammar[new NonTerminal('S')].Should().BeEquivalentTo(expectedSRules);
            grammar[new NonTerminal('A')].Should().BeEquivalentTo(expectedARules);
        }

        [Test]
        public static void CorrectParse_SimplePrecedenceGrammar()
        {
            var rules = new[]
            {
                "S aSSb",
                "S c"
            };

            var grammar = GrammarParser.Parse(rules);

            var expectedSRules = new[]
            {
                new Rule(new NonTerminal('S'), new Terminal('a'), new NonTerminal('S'), new NonTerminal('S'), new Terminal('b')),
                new Rule(new NonTerminal('S'), new Terminal('c')),
            };

            grammar[new NonTerminal('S')].Should().BeEquivalentTo(expectedSRules);
        }
    }
}