using System;

namespace Kosystem.Shared
{
    public readonly struct NewRoomModel : IEquatable<NewRoomModel>
    {
        public string Name { get; }

        public NewRoomModel(string name)
        {
            Name = name;
        }

        public override bool Equals(object? obj)
        {
            return obj is NewRoomModel model && Equals(model);
        }

        public bool Equals(NewRoomModel other)
        {
            return Name == other.Name;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Name);
        }

        public static bool operator ==(NewRoomModel left, NewRoomModel right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(NewRoomModel left, NewRoomModel right)
        {
            return !(left == right);
        }
    }
}
