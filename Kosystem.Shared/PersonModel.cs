using System;

namespace Kosystem.Shared
{
    public class PersonModel
    {
        public int RoomId { get; set; }

        public string Name { get; set; }

        public DateTime? EnqueuedAt { get; set; }

        public PersonModel(int roomId, string name)
        {
            RoomId = roomId;
            Name = name;
        }
    }
}
