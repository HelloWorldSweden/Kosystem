using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Kosystem.Core
{
    public interface IRoomRepository
    {
        void RegisterRoom(Room room);

        void UnregisterRoom(Room room);

        void RegisterUser(Room room, User user);

        void UnregisterUser(Room room);

        bool EnqueueUser(Room room);

        bool DequeueUser(Room room);

        ICollection<User> ListIdleInRoom(Room room);

        IList<User> ListQueueInRoom(Room room);
    }
    public interface IRoomService
    {
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
