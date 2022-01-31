using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace BetterPreOrderTraverseVisitor
{
    public class OptimizedSimplePriorityQueue<T>
    {
        private readonly List<Entry> _list;
        private int _nextSeqNo;

        public int Count => _list.Count;

        public OptimizedSimplePriorityQueue(int capacity)
        {
            _list = new List<Entry>(capacity);
        }

        public void Enqueue(T value, int priority)
        {
            _list.Add(new Entry(_nextSeqNo++, priority, value));

            var current = _list.Count - 1;

            while (current > 0)
            {
                var parent = (current - 1) >> 1;

                if (Compare(_list[parent], _list[current]) >= 0)
                {
                    break;
                }

                // ReSharper disable once SwapViaDeconstruction
                var tmp = _list[current];
                _list[current] = _list[parent];
                _list[parent] = tmp;

                current = parent;
            }
        }

        public T Dequeue(out int priority)
        {
            if (!TryDequeue(out var value, out priority))
            {
                throw new InvalidOperationException();
            }

            return value;
        }

        public bool TryDequeue(out T value, out int priority)
        {
            if (_list.Count == 0)
            {
                value = default;
                priority = default;
                return false;
            }

            var tmp = _list[0];
            value = tmp.Value;
            priority = tmp.Priority;

            if (_list.Count == 1)
            {
                _list.RemoveAt(0);
                return true;
            }

            _list[0] = _list[_list.Count - 1];
            _list.RemoveAt(_list.Count - 1);

            for (var current = 0;;)
            {
                var largest = current;

                // left
                var child = (current << 1) + 1;

                if (child < _list.Count && Compare(_list[child], _list[largest]) > 0)
                {
                    largest = child;
                }

                // right
                child += 1;

                if (child < _list.Count && Compare(_list[child], _list[largest]) > 0)
                {
                    largest = child;
                }

                if (largest == current)
                {
                    break;
                }

                tmp = _list[current];
                _list[current] = _list[largest];
                _list[largest] = tmp;

                current = largest;
            }

            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int Compare(Entry a, Entry b)
        {
            return a.Priority != b.Priority ? a.Priority - b.Priority : b.SeqNo - a.SeqNo;
        }

        private readonly struct Entry
        {
            public readonly int SeqNo;
            public readonly int Priority;
            public readonly T Value;

            public Entry(int seqNo, int priority, T value)
            {
                SeqNo = seqNo;
                Priority = priority;
                Value = value;
            }
        }
    }
}