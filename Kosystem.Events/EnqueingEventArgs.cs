using System;
using Kosystem.Shared;

namespace Kosystem.Events
{
    public class EnqueingEventArgs : EventArgs
    {
        public PersonModel Person { get; set; }

        public RoomModel Room { get; set; }

        public EnqueingEventArgs(PersonModel person, RoomModel room)
        {
            Person = person;
            Room = room;
        }
    }
}
