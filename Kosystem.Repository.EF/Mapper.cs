using Kosystem.Shared;

namespace Kosystem.Repository.EF
{
    internal static class Mapper
    {
        public static PersonModel ToPersonModel(this Person person)
        {
            return new PersonModel(person.Id, person.Name)
            {
                RoomId = person.Room?.Id,
            };
        }

        public static RoomModel ToRoomModel(this Room room)
        {
            return new RoomModel(room.Id, room.Name);
        }
    }
}
