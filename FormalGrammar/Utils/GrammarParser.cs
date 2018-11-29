using System;
using System.Linq;
using System.Text.RegularExpressions;
using FormalGrammar.Model;

namespace FormalGrammar.Utils
{
    public static class GrammarParser
    {
        public static Grammar Parse(string[] rules)
        {
            var parsed = rules.Select(Parse).ToArray();
            var axiom = parsed.First().Left;
            return new Grammar(axiom, parsed);
        }

        private static Rule Parse(string rule)
        {
            var math = RuleRegex.Match(rule);
            if (!math.Success)
                throw new InvalidOperationException($"Правило '{rule}' имеет неверный формат");
            var left = new NonTerminal(math.Groups["NonTerm"].Value.Single());
            var right = math.Groups["Right"].Value.Select(s => char.IsUpper(s) ? (Symbol)new NonTerminal(s) : new Terminal(s)).ToArray();
            return new Rule(left, right);
        }

        private static readonly Regex RuleRegex = new Regex(@"^\s*(?<NonTerm>\w)\s(?<Right>\w+)\s*$");
    }
}