using System.Collections.Generic;
using System.Linq;

namespace FormalGrammar.Model
{
    public class Rule
    {
        public readonly NonTerminal Left;
        public readonly IReadOnlyList<Symbol> Right;

        public Rule(NonTerminal left, params Symbol[] right)
        {
            Left = left;
            Right = right;
        }

        public override bool Equals(object obj)
        {
            return obj != null && obj is Rule r && r.Left == Left && r.Right.Zip(Right, (a, b) => a == b).All(b => b);
        }
        public override int GetHashCode() => Left.GetHashCode();
        public override string ToString() => $"{Left} -> {string.Join("", Right)}";
    }
}