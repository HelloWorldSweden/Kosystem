using Kosystem.Core.DTO;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Kosystem.Core
{
    public interface IRoomService
    {
        Task<Room> GetRoomByIdAsync(Guid roomId, CancellationToken cancellationToken = default);
        Task<User> GetUserByIdAsync(Guid userId, CancellationToken cancellationToken = default);

        Task<IList<Room>> ListRoomsAsync(CancellationToken cancellationToken = default);

        Task<Guid> RegisterRoomAsync(RoomCreationDTO room, CancellationToken cancellationToken = default);

        Task UnregisterRoomAsync(Guid roomId, CancellationToken cancellationToken = default);

        Task<Guid> RegisterUserAsync(UserCreationDTO user, CancellationToken cancellationToken = default);

        Task UnregisterUserAsync(Guid userId, CancellationToken cancellationToken = default);

        Task<bool> EnqueueUserAsync(Guid userId, CancellationToken cancellationToken = default);

        Task<bool> DequeueUserAsync(Guid userId, CancellationToken cancellationToken = default);

        Task<ICollection<User>> ListIdleInRoomAsync(Guid roomId, CancellationToken cancellationToken = default);

        Task<IList<User>> ListQueueInRoomAsync(Guid roomId, CancellationToken cancellationToken = default);
    }
}
