using System;

namespace Kosystem.Shared
{
    public readonly struct UpdateRoomModel : IEquatable<UpdateRoomModel>
    {
        public int Id { get; }

        public string Name { get; }

        public UpdateRoomModel(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public override bool Equals(object? obj)
        {
            return obj is UpdateRoomModel model && Equals(model);
        }

        public bool Equals(UpdateRoomModel other)
        {
            return Id == other.Id
                && Name == other.Name;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Name);
        }

        public static bool operator ==(UpdateRoomModel left, UpdateRoomModel right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(UpdateRoomModel left, UpdateRoomModel right)
        {
            return !(left == right);
        }
    }
}
