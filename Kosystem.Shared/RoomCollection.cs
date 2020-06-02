using System.Collections.Concurrent;

namespace Kosystem.States
{
    public class RoomCollection
    {
        private readonly ConcurrentDictionary<int, Room> _rooms = new ConcurrentDictionary<int, Room>();

        public Room? TryGetById(int id)
        {
            return _rooms.TryGetValue(id, out var room) ? room : null;
        }

        public bool TryAdd(Room room)
        {
            return _rooms.TryAdd(room.Id, room);
        }
    }
}
