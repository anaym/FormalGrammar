using System.Collections.Generic;
using System.Linq;

namespace FormalGrammar.Model
{
    public class Grammar
    {
        public Grammar(NonTerminal axiom, Rule[] rules)
        {
            this.axiom = axiom;
            this.rules = rules.GroupBy(r => r.Left).ToDictionary(g => g.Key, g => (IReadOnlyList<Rule>) g.ToArray());
        }

        public IReadOnlyList<Rule> this[NonTerminal left] => rules.TryGetValue(left, out var rls) ? rls : new Rule[0];

        private readonly NonTerminal axiom;
        private readonly Dictionary<NonTerminal, IReadOnlyList<Rule>> rules;
    }
}