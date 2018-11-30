using System;
using System.Collections.Generic;
using System.Linq;
using FormalGrammar.Utils;

namespace FormalGrammar.Model
{
    public static class BfsHelper
    {
        public static bool HasPath<T>(T start, T end, Func<T, IEnumerable<T>> getNodes)
        {
            var visited = new HashSet<T> {start};
            var queue = new Queue<T>();
            queue.Enqueue(start);

            while (queue.Any())
            {
                var current = queue.Dequeue();
                foreach (var next in getNodes(current))
                {
                    if (next.Equals(end))
                        return true;
                    if (!visited.Contains(next))
                    {
                        queue.Enqueue(next);
                        visited.Add(next);
                    }
                }
            }

            return false;
        }
    }
}