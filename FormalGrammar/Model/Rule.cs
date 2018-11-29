using System.Collections.Generic;

namespace FormalGrammar.Model
{
    public class Rule
    {
        public readonly NonTerminal Left;
        public readonly IReadOnlyList<Symbol> Right;

        public Rule(NonTerminal left, IReadOnlyList<Symbol> right)
        {
            Left = left;
            Right = right;
        }
    }
}