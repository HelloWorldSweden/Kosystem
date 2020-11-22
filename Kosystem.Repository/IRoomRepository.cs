using System.Collections.Generic;
using Kosystem.Shared;

namespace Kosystem.Repository
{
    public interface IRoomRepository
    {
        RoomModel CreateRoom(NewRoomModel newRoom);

        RemoveResult DeleteRoom(int roomId);

        RoomModel? UpdateRoom(UpdateRoomModel patch);

        RoomModel? FindRoom(int roomId);

        IReadOnlyCollection<RoomModel> FindRooms();

        AddResult AddPersonToRoom(int roomId, int personId);

        RemoveResult RemovePersonFromRoom(int roomId, int personId);

        IReadOnlyCollection<PersonModel> FindPeopleInRoom(int roomId);
    }
}
