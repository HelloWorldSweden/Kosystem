using System;
using Kosystem.Shared;

namespace Kosystem.Events
{
    public interface IKosystemEvents
    {
        event EventHandler<EnqueingEventArgs>? Enqueued;
        event EventHandler<EnqueingEventArgs>? Dequeued;

        void OnDequeued(object sender, PersonModel person, RoomModel room);
        void OnEnqueued(object sender, PersonModel person, RoomModel room);
    }
}
