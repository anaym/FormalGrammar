using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using FormalGrammar.Utils;

namespace FormalGrammar.Model
{
    public class GrammarAnalyzer
    {
        public readonly bool IsSimplePrecedence = false;
        public readonly bool IsWeakPrecedence = false;
        public GrammarAnalyzer(GrammarPrecedence grammar)
        {
            this.grammar = grammar;
            IsSimplePrecedence = grammar.IsSimplePrecedence();
            IsWeakPrecedence = IsSimplePrecedence || grammar.IsWeakPrecedence();

            foreach (var rule in grammar.Grammar.GetRules())
                rules.Add(rule.Right.Reverse(), rule);
        }

        public delegate void OnIteration(IEnumerable<Symbol> stack, IEnumerable<Symbol> stringLeft);
        public bool IsAcceptable(IEnumerable<Terminal> terminals, OnIteration onIteration = null)
        {
            onIteration = onIteration ?? ((_, __) => { });

            var terminalsList = terminals.Cast<Symbol>().ToList();
            terminalsList.Add(new End());

            var stack = new Stack<Symbol>();
            stack.Push(new Start());

            var i = 0;
            while (i < terminalsList.Count)
            {
                onIteration(stack, terminalsList.Skip(i));
                var current = terminalsList[i];
                var pair = (stack.Peek(), current);
                if (grammar.IsEqual(pair) || grammar.IsLess(pair))
                {
                    stack.Push(current);
                    i++;
                }
                else if (grammar.IsGreater(pair))
                {
                    if (!rules.TryGetByLongestPrefix(stack, out var rule))
                    {
                        if (stack.Peek() == grammar.Grammar.Axiom && current == new End())
                        {
                            stack.Push(current);
                            onIteration(stack, new Symbol[0]);
                            break;
                        }
                        return false;
                    }

                    stack.PopCount(rule.Right.Count);
                    stack.Push(rule.Left);
                }
                else
                {
                    return false;
                }
            }

            return stack.Count == 3 && stack.Pop() == new End() && stack.Pop() == grammar.Grammar.Axiom && stack.Pop() == new Start();
        }

        private readonly GrammarPrecedence grammar;
        private readonly ArrayIndexedDictionary<Symbol, Rule> rules = new ArrayIndexedDictionary<Symbol, Rule>();
    }
}