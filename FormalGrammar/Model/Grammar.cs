using System.Collections.Generic;
using System.Linq;

namespace FormalGrammar.Model
{
    public class Grammar
    {
        public Grammar(NonTerminal axiom, Rule[] rules)
        {
            Axiom = axiom;
            this.rules = rules.GroupBy(r => r.Left).ToDictionary(g => g.Key, g => (IReadOnlyList<Rule>) g.Distinct().ToArray());
            Symbols = rules.Select(r => r.Left).Concat(rules.SelectMany(r => r.Right)).Distinct().ToList();
            NonTerminals = Symbols.Select(s => s as NonTerminal).Where(s => s != null).ToArray();
            Terminals = Symbols.Select(s => s as Terminal).Where(s => s != null).ToArray();
        }

        public IReadOnlyList<Rule> this[NonTerminal left] => rules.TryGetValue(left, out var rls) ? rls : new Rule[0];
        public readonly NonTerminal Axiom;
        public readonly IReadOnlyCollection<NonTerminal> NonTerminals;
        public readonly IReadOnlyCollection<Terminal> Terminals;
        public readonly IReadOnlyCollection<Symbol> Symbols;

        private readonly Dictionary<NonTerminal, IReadOnlyList<Rule>> rules;
    }
}