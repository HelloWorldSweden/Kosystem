using System;
using Kosystem.Shared;

namespace Kosystem.Events
{
    public class KosystemEvents : IKosystemEvents
    {
        public event EventHandler<EnqueingEventArgs>? Enqueued;
        public event EventHandler<EnqueingEventArgs>? Dequeued;

        public void OnEnqueued(object sender, PersonModel person, RoomModel room)
        {
            Enqueued?.Invoke(sender, new EnqueingEventArgs(person, room));
        }

        public void OnDequeued(object sender, PersonModel person, RoomModel room)
        {
            Dequeued?.Invoke(sender, new EnqueingEventArgs(person, room));
        }
    }
}
