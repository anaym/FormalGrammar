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
    }
}