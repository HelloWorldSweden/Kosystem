using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
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

        public AddResult AddPersonToRoom(int roomId, int personId)
        {
            var peopleInRoom = _peoplePerRoom.GetOrAdd(roomId, _ => new ConcurrentHashSet<int>());
            if (!peopleInRoom.Add(personId))
            {
                return AddResult.AlreadyAdded;
            }

            if (!_people.TryGetValue(personId, out var person) || person is null)
            {
                RemovePersonFromRoom(roomId, personId);
                return AddResult.UnableToAdd;
            }

            var newPerson = person with { RoomId = roomId };
            if (!_people.TryUpdate(personId, newPerson, person))
            {
                RemovePersonFromRoom(roomId, personId);
                return AddResult.UnableToAdd;
            }

            return AddResult.OK;
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

        public RemoveResult DeletePerson(int personId)
        {
            return _people.TryRemove(personId, out _)
                ? RemoveResult.OK
                : RemoveResult.AlreadyRemoved;
        }

        public RemoveResult DeleteRoom(int roomId)
        {
            var peopleInRoom = FindPeopleInRoom(roomId);

            foreach (var person in peopleInRoom)
            {
                _people.TryUpdate(person.Id, person.WithoutRoom(), person);
            }

            _peoplePerRoom.TryRemove(roomId, out _);
            return _rooms.TryRemove(roomId, out _)
                ? RemoveResult.OK
                : RemoveResult.AlreadyRemoved;
        }

        public RemoveResult DequeuePerson(int personId)
        {
            var person = FindPerson(personId);
            if (person?.IsEnqueued is true)
            {
                return _people.TryUpdate(personId, person.AsDequeued(), person)
                    ? RemoveResult.OK
                    : RemoveResult.AlreadyRemoved;
            }

            return RemoveResult.AlreadyRemoved;
        }

        public AddResult EnqueuePerson(int personId)
        {
            var person = FindPerson(personId);
            if (person?.IsEnqueued is false)
            {
                return _people.TryUpdate(personId, person.AsEnqueued(DateTime.Now), person)
                    ? AddResult.OK
                    : AddResult.AlreadyAdded;
            }

            return AddResult.AlreadyAdded;
        }

        public IReadOnlyCollection<PersonModel> FindPeopleInRoom(int roomId)
        {
            if (_peoplePerRoom.TryGetValue(roomId, out var peopleInRoom))
            {
                var list = new List<PersonModel>(peopleInRoom.Count);

                list.AddRange(peopleInRoom.Select(FindPerson).WhereNotNull());

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

            rooms.AddRange(_rooms.Values.WhereNotNull());

            return rooms;
        }

        public RemoveResult RemovePersonFromRoom(int roomId, int personId)
        {
            if (_peoplePerRoom.TryGetValue(roomId, out var peopleInRoom)
                && peopleInRoom.Remove(personId))
            {
                if (!_people.TryGetValue(personId, out var person)
                    || person is null)
                {
                    RemovePersonFromRoom(roomId, personId);
                    return RemoveResult.UnableToRemove;
                }

                var newPerson = person.WithoutRoom();
                if (!_people.TryUpdate(personId, newPerson, person))
                {
                    RemovePersonFromRoom(roomId, personId);
                    return RemoveResult.UnableToRemove;
                }

                return RemoveResult.OK;
            }

            return RemoveResult.AlreadyRemoved;
        }

        public PersonModel? UpdatePerson(UpdatePersonModel patch)
        {
            var person = FindPerson(patch.Id);
            if (person is null)
            {
                return null;
            }

            PersonModel updatedPerson = person.Update(patch);
            if (_people.TryUpdate(person.Id, updatedPerson, person))
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
            if (room is null)
            {
                return null;
            }

            var updatedRoom = room.Update(patch);
            if (_rooms.TryUpdate(room.Id, updatedRoom, room))
            {
                return updatedRoom;
            }
            else
            {
                return null;
            }
        }

        private int ClaimNextUniqueId<T>(ConcurrentDictionary<int, T?> dictionary, int maxAttempts = 64)
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
            while (!dictionary.TryAdd(id, default));

            return id;
        }
    }
}
