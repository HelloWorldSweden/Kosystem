using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Kosystem.Utility
{
    public class ConcurrentHashSet<T> : ICollection<T>
    {
        private readonly ConcurrentDictionary<int, T> _dictionary = new ConcurrentDictionary<int, T>();

        public int Count => _dictionary.Count;

        public bool IsReadOnly => false;

        public bool Add(T item)
        {
            if (item is null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            return _dictionary.TryAdd(item.GetHashCode(), item);
        }

        void ICollection<T>.Add(T item)
        {
            Add(item);
        }

        public bool Remove(T item)
        {
            if (item is null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            return _dictionary.TryRemove(item.GetHashCode(), out _);
        }

        public void Clear()
        {
            _dictionary.Clear();
        }

        public bool Contains(T item)
        {
            if (item is null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            return _dictionary.ContainsKey(item.GetHashCode());
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            ((ICollection<T>)_dictionary).CopyTo(array, arrayIndex);
        }

        public void ExceptWith(IEnumerable<T> other)
        {
            foreach (var item in other)
            {
                if (item is null)
                {
                    continue;
                }

                _dictionary.TryRemove(item.GetHashCode(), out _);
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _dictionary.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
