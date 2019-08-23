using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Kosystem.Core
{
    public interface IRoomService
    {
        Task<IList<Room>> ListRoomsAsync(CancellationToken cancellationToken = default);

        Task RegisterRoomAsync(Room room, CancellationToken cancellationToken = default);

        Task UnregisterRoomAsync(Room room, CancellationToken cancellationToken = default);

        Task RegisterUserAsync(Room room, User user, CancellationToken cancellationToken = default);

        Task UnregisterUserAsync(User user, CancellationToken cancellationToken = default);

        Task<bool> EnqueueUserAsync(User user, CancellationToken cancellationToken = default);

        Task<bool> DequeueUserAsync(User user, CancellationToken cancellationToken = default);

        Task<ICollection<User>> ListIdleInRoomAsync(Room room, CancellationToken cancellationToken = default);

        Task<IList<User>> ListQueueInRoomAsync(Room room, CancellationToken cancellationToken = default);
    }
}
