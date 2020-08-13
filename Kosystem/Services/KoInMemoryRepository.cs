using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Kosystem.Services;
using Kosystem.Shared;
using Kosystem.Utility;

namespace Kosystem.States
{
    public class KoInMemoryRepository : IRoomRepository, IPersonRepository
    {
        private const int ID_LIMIT = 9999;

        private readonly ConcurrentIdGenerator _idGenerator = new ConcurrentIdGenerator();
        private readonly ConcurrentDictionary<int, RoomModel?> _rooms = new ConcurrentDictionary<int, RoomModel?>();
        private readonly ConcurrentDictionary<int, PersonModel?> _people = new ConcurrentDictionary<int, PersonModel?>();
        private readonly ConcurrentDictionary<int, ConcurrentHashSet<int>> _peoplePerRoom = new ConcurrentDictionary<int, ConcurrentHashSet<int>>();

        public bool AddPersonToRoom(int roomId, int personId)
        {
            var peopleInRoom = _peoplePerRoom.GetOrAdd(roomId, _ => new ConcurrentHashSet<int>());
            return peopleInRoom.Add(personId);
        }

        public PersonModel CreatePerson(NewPersonModel newPerson)
        {
            int id = ClaimNextUniqueId(_people);

            var person = new PersonModel(id, newPerson.Name);
            _people[id] = person;

            return person;
        }

        public RoomModel CreateRoom(NewRoomModel newRoom)
        {
            var id = ClaimNextUniqueId(_rooms);

            var room = new RoomModel(id, newRoom.Name);
            _rooms[id] = room;

            return room;
        }

        public bool DeletePerson(int personId)
        {
            return _people.TryRemove(personId, out _);
        }

        public bool DeleteRoom(int roomId)
        {
            var peopleInRoom = FindPeopleInRoom(roomId);

            foreach (var person in peopleInRoom)
            {
                _people.TryUpdate(person.Id, person.WithoutRoom(), person);
            }

            _peoplePerRoom.TryRemove(roomId, out _);
            return _people.TryRemove(roomId, out _);
        }

        public bool DequeuePerson(int personId)
        {
            var person = FindPerson(personId);
            if (person.HasValue && person.Value.IsEnqueued)
            {
                return _people.TryUpdate(personId, person.Value.AsDequeued(), person);
            }

            return false;
        }

        public bool EnqueuePerson(int personId)
        {
            var person = FindPerson(personId);
            if (person.HasValue && !person.Value.IsEnqueued)
            {
                return _people.TryUpdate(personId, person.Value.AsEnqueued(DateTime.Now), person);
            }

            return false;
        }

        public IReadOnlyCollection<PersonModel> FindPeopleInRoom(int roomId)
        {
            if (_peoplePerRoom.TryGetValue(roomId, out var peopleInRoom))
            {
                var list = new List<PersonModel>(peopleInRoom.Count);

                foreach (var personId in peopleInRoom)
                {
                    var person = FindPerson(personId);
                    if (person.HasValue)
                    {
                        list.Add(person.Value);
                    }
                }

                return list;
            }

            return Array.Empty<PersonModel>();
        }

        public PersonModel? FindPerson(int personId)
        {
            return _people.TryGetValue(personId, out var person) ? person : null;
        }

        public RoomModel? FindRoom(int roomId)
        {
            return _rooms.TryGetValue(roomId, out var room) ? room : null;
        }

        public IReadOnlyCollection<RoomModel> FindRooms()
        {
            var rooms = new List<RoomModel>(_rooms.Count);

            foreach (var room in _rooms.Values)
            {
                if (room.HasValue)
                {
                    rooms.Add(room.Value);
                }
            }

            return rooms;
        }

        public bool RemovePersonFromRoom(int roomId, int personId)
        {
            if (_peoplePerRoom.TryGetValue(roomId, out var peopleInRoom))
            {
                return peopleInRoom.Remove(personId);
            }

            return false;
        }

        public PersonModel? UpdatePerson(UpdatePersonModel patch)
        {
            var person = FindPerson(patch.Id);
            if (!person.HasValue)
            {
                return null;
            }

            PersonModel updatedPerson = person.Value.Update(patch);
            if (_people.TryUpdate(person.Value.Id, updatedPerson, person))
            {
                return updatedPerson;
            }
            else
            {
                return null;
            }
        }

        public RoomModel? UpdateRoom(UpdateRoomModel patch)
        {
            var room = FindRoom(patch.Id);
            if (!room.HasValue)
            {
                return null;
            }

            var updatedRoom = room.Value.Update(patch);
            if (_rooms.TryUpdate(room.Value.Id, updatedRoom, room))
            {
                return updatedRoom;
            }
            else
            {
                return null;
            }
        }

        private int ClaimNextUniqueId<T>(ConcurrentDictionary<int, T?> dictionary, int maxAttempts = 64)
            where T : struct
        {
            int id;
            int attempts = 0;
            do
            {
                id = _idGenerator.Next(ID_LIMIT);
                if (attempts++ > maxAttempts)
                {
                    throw new InvalidOperationException("Unable to generate unique ID. Perhaps there's too many existing items?");
                }
            }
            while (!dictionary.TryAdd(id, null!));

            return id;
        }
    }
}
