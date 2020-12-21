using System;
using Kosystem.Shared;

namespace Kosystem.Events
{
    public abstract class QueingEventArgs : EventArgs
    {
        public PersonModel? Person { get; init; }

        public RoomModel? Room { get; init; }
    }
}
