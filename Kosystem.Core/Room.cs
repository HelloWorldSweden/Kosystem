using System;
using System.Collections.Generic;

namespace Kosystem.Core
{
    public class Room
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public ICollection<User> Users { get; set; }

        public DateTimeOffset CreatedAt { get; set; }

        public DateTimeOffset? ChangedAt { get; set; }

        public override bool Equals(object obj)
        {
            if (obj is Room room)
            {
                return room.Id == Id &&
                    room.Name == Name &&
                    Users.Equals(room.Users);
            }

            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            int hash = 17;
            hash = hash * 31 + Id.GetHashCode();
            hash = hash * 31 + Name.GetHashCode();

            foreach (User user in Users)
            {
                hash = hash * 31 + user.GetHashCode();
            }

            return hash;
        }
    }
}
