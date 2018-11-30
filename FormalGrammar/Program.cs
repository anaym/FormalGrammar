using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using FormalGrammar.Model;
using FormalGrammar.Utils;

namespace FormalGrammar
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 2)
            {
                Console.WriteLine($"FormalGrammar.exe [in-filename] [out-filename]");
                return;
            }

            var (rules, strings, ntOrder) = ReadInput(args[0]);

            var grammar = GrammarParser.Parse(rules);
            var grammarPrecedence = new GrammarPrecedence(grammar);

            var view = new GrammarPrecedenceView(s => OrderSymbols(s, ntOrder));
            var table = view.ToTable(grammarPrecedence);

            var analyzer = new GrammarAnalyzer(grammarPrecedence);
            var type = analyzer.IsSimplePrecedence ? "S" : (analyzer.IsWeakPrecedence ? "W" : "N");

            File.WriteAllText(args[1], $"{table}\r\n{type}", Encoding.UTF8);
        }

        private static IOrderedEnumerable<Symbol> OrderSymbols(IEnumerable<Symbol> symbols, IReadOnlyDictionary<char, int> ntOrder)
        {
            return symbols
                .OrderBy(s => s == null ? 0 : (s is End || s is Start ? 2 : 1))
                .ThenBy(s => s is NonTerminal ? 0 : 1)
                .ThenBy(s => s != null && ntOrder.TryGetValue(s.Value, out var i) ? i : int.MaxValue)
                .ThenBy(s => s?.Value);
        }

        private static (string[] Rules, string[] Strings, Dictionary<char, int> NTOrder) ReadInput(string fileName)
        {
            var input = File.ReadAllLines(fileName, Encoding.UTF8);
            var rules = input
                .TakeWhile(l => !string.IsNullOrWhiteSpace(l))
                .ToArray();
            var strings = input
                .SkipWhile((l => !string.IsNullOrWhiteSpace(l)))
                .Skip(1)
                .Where(l => !string.IsNullOrWhiteSpace(l))
                .ToArray();
            var ntOrder = rules
                .SelectMany(r => r)
                .Where(char.IsUpper)
                .Distinct()
                .Select((c, i) => (c, i))
                .ToDictionary(p => p.Item1, p => p.Item2);
            return (rules, strings, ntOrder);
        }
    }
}
