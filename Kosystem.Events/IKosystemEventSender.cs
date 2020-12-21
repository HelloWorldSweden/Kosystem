using Kosystem.Shared;

namespace Kosystem.Events
{
    public interface IKosystemEventSender
    {
        void OnDequeued(object sender, PersonModel person, RoomModel room);
        void OnEnqueued(object sender, PersonModel person, RoomModel room);
        void OnJoinedRoom(object sender, PersonModel person, RoomModel room);
        void OnLeftRoom(object sender, PersonModel person, RoomModel room);
    }
}
