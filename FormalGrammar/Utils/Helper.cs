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
        public static bool EndsWith<T>(this IReadOnlyList<T> list, IReadOnlyList<T> ending)
        {
            if (list.Count < ending.Count)
                return false;
            for (var i = 0; i < ending.Count; i++)
                if (!Equals(list[list.Count - ending.Count + i], ending[i]))
                    return false;
            return true;
        }
        public static void PopCount<T>(this Stack<T> stack, int count)
        {
            for (var i = 0; i < count; i++)
                stack.Pop();
        }
        public static T SingleOrDefaultSafe<T>(this IEnumerable<T> seq, Predicate<T> predicate)
        {
            var filtered = seq.Where(i => predicate(i)).Take(2).ToArray();
            if (filtered.Length != 1)
                return default(T);
            return filtered.Single();
        }
    }
}