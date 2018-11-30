using System;
using System.Collections.Generic;
using System.Linq;

namespace FormalGrammar.Utils
{
    public static class Helper
    {
        public static HashSet<T> ToHashSet<T>(this IEnumerable<T> seq) => new HashSet<T>(seq);
        public static IEnumerable<T> WhereNot<T>(this IEnumerable<T> seq, Predicate<T> predicate) => seq.Where(i => !predicate(i));
        public static void AddItems<T>(this ICollection<T> collection, IEnumerable<T> items)
        {
            foreach (var item in items)
                collection.Add(item);
        }
        public static bool Add<TK, TV>(this IDictionary<TK, HashSet<TV>> dict, TK key, TV value)
        {
            if (!dict.TryGetValue(key, out var set))
                dict[key] = set = new HashSet<TV>();
            return set.Add(value);
        }
        public static bool All<T>(this IEnumerable<T> seq, Func<T, int, bool> predicate)
        {
            return seq.Select(predicate).All(l => l);
        }
    }
}