using System;

namespace Kosystem.Shared
{
    public readonly struct RoomModel : IEquatable<RoomModel>
    {
        public int Id { get; }

        public string Name { get; }

        public RoomModel(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public RoomModel Update(UpdateRoomModel patch)
        {
            return new RoomModel(Id, patch.Name);
        }

        public override bool Equals(object? obj)
        {
            return obj is RoomModel model && Equals(model);
        }

        public bool Equals(RoomModel other)
        {
            return Id == other.Id;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id);
        }

        public static bool operator ==(RoomModel left, RoomModel right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(RoomModel left, RoomModel right)
        {
            return !(left == right);
        }
    }
}
