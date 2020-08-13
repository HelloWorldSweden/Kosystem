using System;

namespace Kosystem.Shared
{
    public readonly struct NewPersonModel : IEquatable<NewPersonModel>
    {
        public string Name { get; }

        public NewPersonModel(string name)
        {
            Name = name;
        }

        public override bool Equals(object? obj)
        {
            return obj is NewPersonModel model && Equals(model);
        }

        public bool Equals(NewPersonModel other)
        {
            return Name == other.Name;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Name);
        }

        public static bool operator ==(NewPersonModel left, NewPersonModel right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(NewPersonModel left, NewPersonModel right)
        {
            return !(left == right);
        }
    }
}
