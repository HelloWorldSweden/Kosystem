using System;
using Kosystem.Shared;

namespace Kosystem.Events
{
    public class RoomAttendanceEventArgs : EventArgs
    {
        public PersonModel Person { get; set; }

        public RoomModel Room { get; set; }

        public RoomAttendanceEventArgs(PersonModel person, RoomModel room)
        {
            Person = person;
            Room = room;
        }
    }
}
