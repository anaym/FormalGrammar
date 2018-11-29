using System.Linq;
using FluentAssertions;
using FormalGrammar.Model;
using FormalGrammar.Utils;
using NUnit.Framework;

namespace FormalGrammar.Tests
{
    [TestFixture]
    public class GrammarPrecedence_Should
    {
        [Test]
        public static void CorrectFindLessAndEqualAndGreateSets_WhenG31()
        {
            var grammar = GrammarParser.Parse("S aFSb", "S c", "F Fb", "F b");
            
            var precedence = new GrammarPrecedence(grammar);

            precedence.Less.Select(p => (p.Item1.Value, p.Item2.Value))
                .Should()
                .BeEquivalentTo(('a', 'F'), ('a', 'b'), ('F', 'a'), ('F', 'c'), ('^', 'S'), ('^', 'a'), ('^', 'c'));

            precedence.Equal.Select(p => (p.Item1.Value, p.Item2.Value))
                .Should()
                .BeEquivalentTo(('a', 'F'), ('F', 'S'), ('S', 'b'), ('F', 'b'));

            precedence.Greater.Select(p => (p.Item1.Value, p.Item2.Value))
                .Should()
                .BeEquivalentTo(('b', 'a'), ('b', 'c'), ('b', 'b'), ('c', 'b'), ('S', '$'), ('b', '$'), ('c', '$'));
        }
    }
}