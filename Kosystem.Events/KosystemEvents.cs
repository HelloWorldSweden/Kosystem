using System;
using Kosystem.Shared;

namespace Kosystem.Events
{
    public class KosystemEvents : IKosystemEventListener, IKosystemEventSender
    {
        public event EventHandler<EnqueingEventArgs>? Enqueued;
        public event EventHandler<EnqueingEventArgs>? Dequeued;
        public event EventHandler<RoomAttendanceEventArgs>? JoinedRoom;
        public event EventHandler<RoomAttendanceEventArgs>? LeftRoom;

        public void OnEnqueued(object sender, PersonModel person, RoomModel room)
        {
            Enqueued?.Invoke(sender, new EnqueingEventArgs(person, room));
        }

        public void OnDequeued(object sender, PersonModel person, RoomModel room)
        {
            Dequeued?.Invoke(sender, new EnqueingEventArgs(person, room));
        }

        public void OnJoinedRoom(object sender, PersonModel person, RoomModel room)
        {
            JoinedRoom?.Invoke(sender, new RoomAttendanceEventArgs(person, room));
        }

        public void OnLeftRoom(object sender, PersonModel person, RoomModel room)
        {
            LeftRoom?.Invoke(sender, new RoomAttendanceEventArgs(person, room));
        }
    }
}
