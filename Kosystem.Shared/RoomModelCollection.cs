using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Kosystem.Shared
{
    public class RoomModelCollection : IReadOnlyCollection<RoomModel>
    {
        private readonly ConcurrentDictionary<int, RoomModel> _rooms = new ConcurrentDictionary<int, RoomModel>();

        public int Count => _rooms.Count;

        public RoomModel? TryGetById(int id)
        {
            return _rooms.TryGetValue(id, out var room) ? room : null;
        }

        public bool TryAdd(RoomModel room)
        {
            return _rooms.TryAdd(room.Id, room);
        }

        public IEnumerator<RoomModel> GetEnumerator()
        {
            return _rooms.Values.OrderBy(o => o.Id).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
