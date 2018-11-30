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
            if (obj == null || !(obj is Rule r))
                return false;
            return r.Left == Left && r.Right.Count == Right.Count && r.Right.Zip(Right, (a, b) => a == b).All(b => b);
        }
        public override int GetHashCode() => Left.GetHashCode();
        public override string ToString() => $"{Left} -> {string.Join("", Right)}";
    }
}