using System.Collections.Generic;
using System.Linq;
using FormalGrammar.Utils;

namespace FormalGrammar.Model
{
    public static class GrammarHelper
    {
        public static HashSet<NonTerminal> GetAnnulmentNonTerminals(this Grammar grammar)
        {
            var annulmentRules = grammar.NonTerminals
                .SelectMany(nt => grammar[nt])
                .Where(r => r.Right.Count == 0);

            var annulmentNonTerminals = annulmentRules.Select(r => r.Left).ToHashSet();

            bool IsAnnuledRule(Rule rule) => rule.Right.All(s => s is NonTerminal nt && annulmentNonTerminals.Contains(nt));

            while (true)
            {
                var newAnnuledNonTerminals = grammar
                    .NonTerminals
                    .WhereNot(annulmentNonTerminals.Contains)
                    .Where(nt => grammar[nt].Any(IsAnnuledRule))
                    .ToList();

                if (!newAnnuledNonTerminals.Any())
                    break;

                annulmentNonTerminals.AddItems(newAnnuledNonTerminals);
            }

            return annulmentNonTerminals;
        }

        public static HashSet<NonTerminal> GetCyclicNonTerminals(this Grammar grammar)
        {
            var annulmentNonTerminals = grammar.GetAnnulmentNonTerminals();

            var rules = grammar.NonTerminals.SelectMany(nt => grammar[nt])
                .Where(r => r.Right.All(s => s is NonTerminal))
                .Where(r => r.Right.Any())
                .ToHashSet();

            var chain = new Dictionary<NonTerminal, HashSet<NonTerminal>>();

            foreach (var rule in rules)
            {
                var right = rule.Right.Cast<NonTerminal>().ToArray();
                if (right.Length == 1)
                    chain.Add(rule.Left, right.Single());
                else if (right.All(annulmentNonTerminals.Contains))
                    foreach (var nt in right)
                        chain.Add(rule.Left, nt);
                else if (right.SingleOrDefault(n => !annulmentNonTerminals.Contains(n)) is var nt)
                    chain.Add(rule.Left, nt);
            }

            var result = new HashSet<NonTerminal>();
            foreach (var nonTerminal in chain.Keys.Select(k => k).Distinct())
            {
                if (BfsHelper.HasPath(nonTerminal, nonTerminal, nt => chain.TryGetValue(nt, out var nxt) ? nxt : Enumerable.Empty<NonTerminal>()))
                    result.Add(nonTerminal);
            }

            return result;
        }

        public static bool IsReversibleGrammar(this Grammar grammar)
        {
            foreach (var a in grammar.GetRules())
            foreach (var b in grammar.GetRules().WhereNot(b => Equals(b, a)).Where(b => a.Right.Count == b.Right.Count))
                if (a.Right.Zip(b.Right, (ia, ib) => ia == ib).All(l => l))
                    return false;
            return true;
        }

        public static IEnumerable<Rule> GetRules(this Grammar grammar)
        {
            return grammar.NonTerminals.SelectMany(nt => grammar[nt]);
        }

        public static bool IsWeakPrecedence(this GrammarPrecedence grammarPrecedence)
        {
            if (grammarPrecedence.Grammar.GetCyclicNonTerminals().Any())
                return false;
            if (!grammarPrecedence.Grammar.IsReversibleGrammar())
                return false;
            if (grammarPrecedence.Equal.Any(grammarPrecedence.IsGreater))
                return false;
            if (grammarPrecedence.Less.Any(grammarPrecedence.IsGreater))
                return false;
            if (grammarPrecedence.Less.Concat(grammarPrecedence.Greater).Any(grammarPrecedence.HasCommonEnding))
                return false;
            return true;
        }

        public static bool IsSimplePrecedence(this GrammarPrecedence grammarPrecedence)
        {
            if (grammarPrecedence.Grammar.GetCyclicNonTerminals().Any())
                return false;
            if (!grammarPrecedence.Grammar.IsReversibleGrammar())
                return false;
            foreach (var a in grammarPrecedence.Grammar.Symbols)
            foreach (var b in grammarPrecedence.Grammar.Symbols)
            {
                var pair = (a, b);
                var isLess = grammarPrecedence.IsLess(pair);
                var isEqual = grammarPrecedence.IsEqual(pair);
                var isGreater = grammarPrecedence.IsGreater(pair);
                if (isLess && isEqual || isEqual && isGreater || isLess && isGreater)
                    return false;
            }
            return true;
        }

        private static bool HasCommonEnding(this GrammarPrecedence grammarPrecedence, (Symbol X, Symbol B) pair)
        {
            if (!(pair.B is NonTerminal b))
                return false;
            foreach (var rule in grammarPrecedence.Grammar[b])
            {
                var right = new[] { pair.X }.Concat(rule.Right).ToList();
                if (grammarPrecedence.Grammar.GetRules().Any(r => r.Right.EndsWith(right)))
                    return true;
            }
            return false;
        }
    }
}