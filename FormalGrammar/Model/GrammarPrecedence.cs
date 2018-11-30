using System.Collections.Generic;
using System.Linq;

namespace FormalGrammar.Model
{
    public class GrammarPrecedence
    {
        public readonly Grammar Grammar;

        public IEnumerable<(Symbol, Symbol)> Less => less;
        public IEnumerable<(Symbol, Symbol)> Equal => equal;
        public IEnumerable<(Symbol, Symbol)> Greater => greater;

        public bool IsLess((Symbol, Symbol) pair) => less.Contains(pair);
        public bool IsEqual((Symbol, Symbol) pair) => equal.Contains(pair);
        public bool IsGreater((Symbol, Symbol) pair) => greater.Contains(pair);

        public GrammarPrecedence(Grammar grammar)
        {
            Grammar = grammar;

            var first = grammar.NonTerminals.ToDictionary(s => s, grammar.GetAllFirstSymbols);
            var last = grammar.NonTerminals.ToDictionary(s => s, grammar.GetAllLastSymbols);

            foreach (var rule in grammar.NonTerminals.SelectMany(nt => grammar[nt]))
                for (var i = 0; i < rule.Right.Count - 1; i++)
                    equal.Add((rule.Right[i], rule.Right[i + 1]));

            foreach (var (x, z) in equal)
                if (z is NonTerminal nt)
                    foreach (var y in first[nt])
                        less.Add((x, y));

            foreach (var (z1, z2) in equal)
                if (z1 is NonTerminal z1Nt)
                    foreach (var x in last[z1Nt])
                        if (z2 is Terminal)
                            greater.Add((x, z2));
                        else if (z2 is NonTerminal z2Nt)
                            foreach (var y in first[z2Nt])
                                greater.Add((x, y));

            less.Add((new Start(), grammar.Axiom));
            foreach (var f in first[grammar.Axiom])
                less.Add((new Start(), f));

            greater.Add((grammar.Axiom, new End()));
            foreach (var l in last[grammar.Axiom])
                greater.Add((l, new End()));
        }

        private readonly HashSet<(Symbol, Symbol)> less = new HashSet<(Symbol, Symbol)>();
        private readonly HashSet<(Symbol, Symbol)> equal = new HashSet<(Symbol, Symbol)>();
        private readonly HashSet<(Symbol, Symbol)> greater = new HashSet<(Symbol, Symbol)>();
    }
}