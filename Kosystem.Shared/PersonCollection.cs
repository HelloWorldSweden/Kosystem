using System.Collections.Concurrent;

namespace Kosystem.States
{
    public class PersonCollection
    {
        private readonly ConcurrentDictionary<string, Person> _people = new ConcurrentDictionary<string, Person>();

        public Person? TryGetByName(string? name)
        {
            if (name is null)
            {
                return null;
            }

            return _people.TryGetValue(name, out var person) ? person : null;
        }

        public bool TryAdd(Person person)
        {
            return _people.TryAdd(person.Name, person);
        }
    }
}
