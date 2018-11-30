using System;
using System.Collections.Generic;
using System.Linq;
using FormalGrammar.Model;

namespace FormalGrammar.Utils
{
    public class GrammarPrecedenceView
    {
        public GrammarPrecedenceView(Func<IEnumerable<Symbol>, IOrderedEnumerable<Symbol>> orderSymbols) => this.orderSymbols = orderSymbols;

        public string ToTable(GrammarPrecedence grammarPrecedence)
        {
            var columnSymbols = orderSymbols(new[] {default(Symbol), new End()}.Concat(grammarPrecedence.Grammar.Symbols)).ToArray();
            var rowSymbols = orderSymbols(new[] {default(Symbol), new Start()}.Concat(grammarPrecedence.Grammar.Symbols)).ToArray();

            var rows = rowSymbols.Select(r => columnSymbols.Select(c => RenderCell(r, c, grammarPrecedence)).ToArray()).ToArray();

            return rows.TableToString();
        }

        private static string RenderCell(Symbol row, Symbol column, GrammarPrecedence grammarPrecedence)
        {
            if (row == null)
                return column?.Value.ToString() ?? "";
            if (column == null)
                return row.Value.ToString();

            var pair = (row, column);

            var result = "";
            if (grammarPrecedence.Less.Contains(pair))
                result += "<";
            if (grammarPrecedence.Greater.Contains(pair))
                result += ">";
            if (grammarPrecedence.Equal.Contains(pair))
                result += "=";

            return string.IsNullOrWhiteSpace(result) ? "." : result;
        }

        private readonly Func<IEnumerable<Symbol>, IOrderedEnumerable<Symbol>> orderSymbols;
    }
}