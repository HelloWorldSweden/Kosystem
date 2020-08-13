using System.Collections.Generic;
using Kosystem.Shared;

namespace Kosystem.Services
{
    public interface IRoomRepository
    {
        RoomModel CreateRoom(NewRoomModel newRoom);

        bool DeleteRoom(int roomId);

        RoomModel? UpdateRoom(UpdateRoomModel patch);

        RoomModel? FindRoom(int roomId);

        IReadOnlyCollection<RoomModel> FindRooms();

        bool AddPersonToRoom(int roomId, int personId);

        bool RemovePersonFromRoom(int roomId, int personId);

        IReadOnlyCollection<PersonModel> FindPeopleInRoom(int roomId);
    }
}
