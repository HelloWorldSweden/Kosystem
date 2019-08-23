using System;

namespace Kosystem.Core
{
    public class User
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public DateTimeOffset CreatedAt { get; set; }

        public DateTimeOffset? EnqueuedAt { get; set; }
    }
}