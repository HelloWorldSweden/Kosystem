using System;

namespace Kosystem.Shared
{
    public record PersonModel(int Id, string Name)
    {
        public int RoomId { get; init; } = -1;

        public DateTime? EnqueuedAt { get; init; }

        public bool IsInRoom => RoomId > 0;

        public bool IsEnqueued => EnqueuedAt.HasValue;

        public PersonModel WithoutRoom()
        {
            return this with { RoomId = -1 };
        }

        public PersonModel AsEnqueued(DateTime enqueuedAt)
        {
            return this with { EnqueuedAt = enqueuedAt };
        }

        public PersonModel AsDequeued()
        {
            return this with { EnqueuedAt = null };
        }

        public PersonModel Update(UpdatePersonModel patch)
        {
            return this with { Name = patch.Name };
        }
    }
}
