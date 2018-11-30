using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace FormalGrammar.Utils
{
    public class ArrayIndexedDictionary<TK, TV>
    {
        public void Add(IEnumerable<TK> key, TV value)
        {
            var current = root;
            foreach (var k in key)
            {
                if (!current.Next.TryGetValue(k, out var next))
                    current.Next[k] = next = new Node();

                current = next;
            }
            
            if (current.HasValue)
                throw new InvalidOperationException($"Key alredy exists");

            current.Value = value;
            current.HasValue = true;
        }

        public bool TryGetByLongestPrefix(IEnumerable<TK> key, out TV value)
        {
            var current = root;
            var founded = false;
            value = default(TV);

            foreach (var k in key)
            {
                if (current.HasValue)
                {
                    founded = true;
                    value = current.Value;
                }

                if (!current.Next.TryGetValue(k, out current))
                    break;
            }

            return founded;
        }

        public class Node
        {
            public readonly Dictionary<TK, Node> Next = new Dictionary<TK, Node>();
            public TV Value;
            public bool HasValue;
        }

        private readonly Node root = new Node();
    }
}