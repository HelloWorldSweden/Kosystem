using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Kosystem.Shared
{
    public class PersonModelCollection : IReadOnlyCollection<PersonModel>
    {
        private static readonly PersonQueueComparer _personQueueComparer = new PersonQueueComparer();

        private readonly ConcurrentDictionary<string, PersonModel> _dictionary = new ConcurrentDictionary<string, PersonModel>();

        public int Count => _dictionary.Count;

        public PersonModel? TryGetByName(string? name)
        {
            if (name is null)
            {
                return null;
            }

            return _dictionary.TryGetValue(name, out var person) ? person : null;
        }

        public bool TryAdd(PersonModel person)
        {
            return _dictionary.TryAdd(person.Name, person);
        }

        public IEnumerator<PersonModel> GetEnumerator()
        {
            return _dictionary.Values
                .OrderBy(o => o.EnqueuedAt, _personQueueComparer)
                .ThenBy(o => o.Name)
                .GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private class PersonQueueComparer : IComparer<DateTime?>
        {
            public int Compare(DateTime? x, DateTime? y)
            {
                // Keep nulls at the end
                if (x is null && y != null)
                {
                    return 1;
                }

                if (x != null && y != null)
                {
                    return x.Value.CompareTo(y.Value);
                }

                return 0;
            }
        }
    }
}
