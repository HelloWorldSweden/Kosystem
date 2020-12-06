using Kosystem.Shared;

namespace Kosystem.Repository.EF
{
    internal static class Mapper
    {
        public static PersonModel ToPersonModel(this DbPerson person)
        {
            return new PersonModel(person.Id, person.Name)
            {
                RoomId = person.Room?.DisplayId,
                EnqueuedAt = person.EnqueuedAt,
            };
        }

        public static RoomModel ToRoomModel(this DbRoom room)
        {
            return new RoomModel(room.DisplayId, room.Name);
        }
    }
}
