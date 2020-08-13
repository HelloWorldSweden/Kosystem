using System;

namespace Kosystem.Shared
{
    public readonly struct PersonModel : IEquatable<PersonModel>
    {
        public int Id { get; }

        public int RoomId { get; }

        public string Name { get; }

        public DateTime? EnqueuedAt { get; }

        public bool IsInRoom => RoomId > 0;

        public bool IsEnqueued => EnqueuedAt.HasValue;

        public PersonModel(int id, string name)
        {
            Id = id;
            Name = name;
            RoomId = -1;
            EnqueuedAt = default;
        }

        public PersonModel(int id, string name, int roomId, DateTime? enqueuedAt)
        {
            Id = id;
            Name = name;
            RoomId = roomId;
            EnqueuedAt = enqueuedAt;
        }

        public PersonModel WithoutRoom()
        {
            return new PersonModel(Id, Name);
        }

        public PersonModel WithRoom(int roomId)
        {
            return new PersonModel(Id, Name, roomId, EnqueuedAt);
        }

        public PersonModel AsEnqueued(DateTime enqueuedAt)
        {
            return new PersonModel(Id, Name, RoomId, enqueuedAt);
        }

        public PersonModel AsDequeued()
        {
            return new PersonModel(Id, Name, RoomId, null);
        }

        public PersonModel Update(UpdatePersonModel patch)
        {
            return new PersonModel(Id, patch.Name, RoomId, EnqueuedAt);
        }

        public override bool Equals(object? obj)
        {
            return obj is PersonModel model && Equals(model);
        }

        public bool Equals(PersonModel other)
        {
            return Id == other.Id;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id);
        }

        public static bool operator ==(PersonModel left, PersonModel right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(PersonModel left, PersonModel right)
        {
            return !(left == right);
        }
    }
}
