using System.Collections.Generic;
using Kosystem.Events;
using Kosystem.Repository;
using Kosystem.Shared;

namespace Kosystem.Services
{
    public class EventAwareRoomRepository : IRoomRepository
    {
        private readonly IRoomRepository _roomRepository;
        private readonly IKosystemEvents _kosystemEvents;

        public EventAwareRoomRepository(IRoomRepository roomRepository, IKosystemEvents kosystemEvents)
        {
            _roomRepository = roomRepository;
            _kosystemEvents = kosystemEvents;
        }

        public AddResult AddPersonToRoom(int roomId, long personId)
        {
            return _roomRepository.AddPersonToRoom(roomId, personId);
        }

        public RoomModel CreateRoom(NewRoomModel newRoom)
        {
            return _roomRepository.CreateRoom(newRoom);
        }

        public RemoveResult DeleteRoom(int roomId)
        {
            return _roomRepository.DeleteRoom(roomId);
        }

        public IReadOnlyCollection<PersonModel> FindPeopleInRoom(int roomId)
        {
            return _roomRepository.FindPeopleInRoom(roomId);
        }

        public RoomModel? FindRoom(int roomId)
        {
            return _roomRepository.FindRoom(roomId);
        }

        public IReadOnlyCollection<RoomModel> FindRooms()
        {
            return _roomRepository.FindRooms();
        }

        public RemoveResult RemovePersonFromRoom(int roomId, long personId)
        {
            return _roomRepository.RemovePersonFromRoom(roomId, personId);
        }

        public RoomModel? UpdateRoom(UpdateRoomModel patch)
        {
            return _roomRepository.UpdateRoom(patch);
        }
    }
}
