using System;

namespace Kosystem.Events
{
    public interface IKosystemEventListener
    {
        event EventHandler<EnqueingEventArgs>? Enqueued;
        event EventHandler<EnqueingEventArgs>? Dequeued;
        event EventHandler<RoomAttendanceEventArgs>? JoinedRoom;
        event EventHandler<RoomAttendanceEventArgs>? LeftRoom;
    }
}
