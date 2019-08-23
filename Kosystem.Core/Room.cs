using System;
using System.Collections.Generic;

namespace Kosystem.Core
{
    public class Room
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public ICollection<User> Users { get; set; }

        public IList<User> Queue { get; set; }

        public DateTimeOffset CreatedAt { get; set; }

        public DateTimeOffset? ChangedAt { get; set; }
    }
}
