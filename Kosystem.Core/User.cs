using System;

namespace Kosystem.Core
{
    public class User
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public DateTimeOffset CreatedAt { get; set; }

        public DateTimeOffset? EnqueuedAt { get; set; }

        public override bool Equals(object obj)
        {
            if (obj is User user)
            {
                return user.Id == Id && user.Name == Name;
            }

            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            int hash = 17;
            hash = hash * 31 + Id.GetHashCode();
            hash = hash * 31 + Name.GetHashCode();
            return hash;
        }
    }
}