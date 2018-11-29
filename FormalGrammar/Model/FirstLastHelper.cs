using System;
using System.Collections.Generic;
using System.Linq;

namespace FormalGrammar.Model
{
    public static class FirstLastHelper
    {
        public static HashSet<Symbol> GetAllFirstSymbols(this Grammar grammar, NonTerminal nonTerminal)
        {
            return grammar.GetSymbols(nonTerminal, rule => rule.First());
        }

        public static HashSet<Symbol> GetAllLastSymbols(this Grammar grammar, NonTerminal nonTerminal)
        {
            return grammar.GetSymbols(nonTerminal, rule => rule.Last());
        }

        private static HashSet<Symbol> GetSymbols(this Grammar grammar, NonTerminal start, Func<IReadOnlyList<Symbol>, Symbol> getNext)
        {
            var symbols = new HashSet<Symbol>();
            var queue = new Queue<NonTerminal>();
            queue.Enqueue(start);
            while (queue.Any())
            {
                var rules = grammar[queue.Dequeue()];
                foreach (var rule in rules.Where(r => r.Right.Any()))
                {
                    var next = getNext(rule.Right);
                    if (next is NonTerminal nt && !symbols.Contains(nt))
                        queue.Enqueue(nt);
                    symbols.Add(next);
                }
            }
            return symbols;
        }
    }
}