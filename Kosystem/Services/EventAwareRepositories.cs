using Kosystem.Events;
using Kosystem.Repository;
using Kosystem.Shared;

namespace Kosystem.Services
{
    public class EventAwareRepositories
    {
        private readonly IPersonRepository _personRepo;
        private readonly IRoomRepository _roomRepo;
        private readonly IKosystemEventSender _events;

        public EventAwareRepositories(
            IPersonRepository personRepo,
            IRoomRepository roomRepo,
            IKosystemEventSender events)
        {
            _personRepo = personRepo;
            _roomRepo = roomRepo;
            _events = events;
        }

        public AddResult EnqueuePerson(PersonModel person, RoomModel room)
        {
            var result = _personRepo.EnqueuePerson(person.Id);
            if (result == AddResult.OK)
            {
                _events.OnEnqueued(this, person, room);
            }
            return result;
        }

        public RemoveResult DequeuePerson(PersonModel person, RoomModel room)
        {
            var result = _personRepo.DequeuePerson(person.Id);
            if (result == RemoveResult.OK)
            {
                _events.OnEnqueued(this, person, room);
            }
            return result;
        }

        public AddResult AddPersonToRoom(PersonModel person, RoomModel room)
        {
            var result = _roomRepo.AddPersonToRoom(room.Id, person.Id);
            if (result == AddResult.OK)
            {
                _events.OnJoinedRoom(this, person, room);
            }
            return result;
        }
    }
}
