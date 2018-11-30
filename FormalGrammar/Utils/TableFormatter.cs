using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FormalGrammar.Utils
{
    public static class TableFormatter
    {
        public static string TableToString(this string[][] rows, string columnGap = " ", string rowGap = "\r\n")
        {
            var widthes = FindColumnWidthes(rows);
            return string.Join(rowGap, rows.Select(r => RowToString(r, columnGap, widthes)));
        }

        private static string RowToString(IEnumerable<string> row, string columnGap, IReadOnlyList<int> widthes)
        {
            return string.Join(columnGap, row.Select((c, i) => (c ?? "").PadRight(widthes[i])));
        }

        private static IReadOnlyList<int> FindColumnWidthes(IEnumerable<IReadOnlyList<string>> rows)
        {
            var columWidthes = new List<int>();
            foreach (var row in rows.Where(r => r != null))
            {
                columWidthes.AddRange(Enumerable.Repeat(0, row.Count - columWidthes.Count));
                for (var i = 0; i < row.Count; i++)
                    columWidthes[i] = Math.Max(columWidthes[i], row[i]?.Length ?? 0);
            }
            return columWidthes;
        }
    }
}