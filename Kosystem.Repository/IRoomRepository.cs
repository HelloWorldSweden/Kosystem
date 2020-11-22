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

        AddResult AddPersonToRoom(int roomId, long personId);

        RemoveResult RemovePersonFromRoom(int roomId, long personId);

        IReadOnlyCollection<PersonModel> FindPeopleInRoom(int roomId);
    }
}
