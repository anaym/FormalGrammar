using System.Linq;
using FormalGrammar.Model;
using FormalGrammar.Utils;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace FormalGrammar.Tests
{
    [TestFixture]
    public class GrammarHelper_Should
    {
        [TestCase("S ;S A;A Se;B S;B gS;C S", ExpectedResult = new [] {'B', 'C', 'S'}, TestName = "Grammar with annulment rules")]
        [TestCase("S ;A S;B A;C B;D C;E D;F E", ExpectedResult = new [] {'A', 'B', 'C', 'D', 'E', 'F', 'S'}, TestName = "Big grammar with annulment rules")]
        [TestCase("A S;S A;A ", ExpectedResult = new [] {'A', 'S'}, TestName = "Grammar with annulment rules and cycle")]
        [TestCase("S a;S A;A Se;B S;B gS;C S", ExpectedResult = new char[0], TestName = "Grammar without annulment rules")]
        public static char[] CorrectFindAnnulmentNonTerminals_When(string rules)
        {
            var grammar = GrammarParser.Parse(rules.Split(';'));
            return grammar.GetAnnulmentNonTerminals().Select(nt => nt.Value).OrderBy(c => c).ToArray();
        }

        [TestCase("S ;S A;A S;B S;B gS;C S", ExpectedResult = new[] { 'A', 'S' }, TestName = "Grammar with annulment rules")]
        [TestCase("S S;S A;A x;A B", ExpectedResult = new[] { 'S' }, TestName = "Grammar with simple cycle (S -> S)")]
        [TestCase("S A;A XYZ;Y XS;X B;B ;Z B", ExpectedResult = new[] { 'A', 'S', 'Y' }, TestName = "Big grammar with annulment rules")]
        [TestCase("A S;S A", ExpectedResult = new[] { 'A', 'S' }, TestName = "Grammar with cycle and without annuled rules")]
        [TestCase("S a;S A;A Se;B S;B gS;C S", ExpectedResult = new char[0], TestName = "Grammar without annulment rules and cycles")]
        public static char[] CorrectFindCyclicNonTerminals_When(string rules)
        {
            var grammar = GrammarParser.Parse(rules.Split(';'));
            return grammar.GetCyclicNonTerminals().Select(nt => nt.Value).OrderBy(c => c).ToArray();
        }
    }
}