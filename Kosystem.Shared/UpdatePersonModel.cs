using System;

namespace Kosystem.Shared
{
    public readonly struct UpdatePersonModel : IEquatable<UpdatePersonModel>
    {
        public int Id { get; }
        public string Name { get; }

        public UpdatePersonModel(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public override bool Equals(object? obj)
        {
            return obj is UpdatePersonModel model && Equals(model);
        }

        public bool Equals(UpdatePersonModel other)
        {
            return Id == other.Id &&
                   Name == other.Name;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Name);
        }

        public static bool operator ==(UpdatePersonModel left, UpdatePersonModel right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(UpdatePersonModel left, UpdatePersonModel right)
        {
            return !(left == right);
        }
    }
}
