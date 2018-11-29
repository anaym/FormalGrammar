using System.Linq;
using FormalGrammar.Model;
using FormalGrammar.Utils;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace FormalGrammar.Tests
{
    [TestFixture]
    public class FirstLastHelper_Should
    {
        [TestCase('S', "S aBcd", "S Su", ExpectedResult = new[] {'S', 'a'}, TestName = "Grammar contains cycle with nonTerminal")]
        [TestCase('S', "S aBcd", "S u", ExpectedResult = new[] {'a', 'u'}, TestName = "Grammar not contains cycle with nonTerminal")]
        [TestCase('S', "S aBcd", "S Fu", "F xTz", "F Fi", ExpectedResult = new[] {'F', 'a', 'x'}, TestName = "Much nonTerminals")]
        [TestCase('S', "S aFSd", "S c", "F Fb", "F b", ExpectedResult = new[] { 'a', 'c' }, TestName = "G-3-1 (S)")]
        [TestCase('F', "S aFSd", "S c", "F Fb", "F b", ExpectedResult = new[] { 'F', 'b' }, TestName = "G-3-1 (F)")]
        public static char[] ReturnCorrectFirstSymbols_When(char nonTerminal, params string[] rules)
        {
            var grammar = GrammarParser.Parse(rules);
            return grammar.GetAllFirstSymbols(new NonTerminal(nonTerminal))
                .Select(c => c.Value)
                .OrderBy(c => c)
                .ToArray();

        }

        [TestCase('S', "S aBcd", "S uS", ExpectedResult = new[] { 'S', 'd' }, TestName = "Grammar contains cycle with nonTerminal")]
        [TestCase('S', "S aBcd", "S u", ExpectedResult = new[] { 'd', 'u' }, TestName = "Grammar not contains cycle with nonTerminal")]
        [TestCase('S', "S aBcd", "S uF", "F xTz", "F iF", ExpectedResult = new[] {'F', 'd', 'z'}, TestName = "Much nonTerminals")]
        [TestCase('S', "S aFSd", "S c", "F Fb", "F b", ExpectedResult = new[] { 'c', 'd' }, TestName = "G-3-1 (S)")]
        [TestCase('F', "S aFSd", "S c", "F Fb", "F b", ExpectedResult = new[] { 'b' }, TestName = "G-3-1 (F)")]
        public static char[] ReturnCorrectLastSymbols_When(char nonTerminal, params string[] rules)
        {
            var grammar = GrammarParser.Parse(rules);
            return grammar.GetAllLastSymbols(new NonTerminal(nonTerminal))
                .Select(c => c.Value)
                .OrderBy(c => c)
                .ToArray();
        }
    }
}