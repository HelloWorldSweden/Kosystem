using Kosystem.Common;
using Kosystem.Core;
using Kosystem.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Kosystem.Data
{
    public class RoomServiceAsyncViaEF : IRoomService
    {
        private readonly RoomDbContext dbContext;

        public RoomServiceAsyncViaEF(RoomDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task RegisterRoomAsync(Room room, CancellationToken cancellationToken = default)
        {
            ValidateRoom(room);

            if (await dbContext.Rooms.AnyAsync(a => a.Id == room.Id, cancellationToken))
            {
                throw new InvalidOperationException($"Room with id '{room.Id}' already exists.");
            }

            var entity = new RoomEntity
            {
                Id = room.Id,
                Name = room.Name,
                CreatedAt = DateTimeOffset.Now
            };

            await dbContext.Rooms.AddAsync(entity, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task UnregisterRoomAsync(Room room, CancellationToken cancellationToken = default)
        {
            RoomEntity entity = await GetRoomAsync(room, cancellationToken);

            if (entity is null)
            {
                throw new InvalidOperationException($"Room with id '{room.Id}' does not exists.");
            }

            dbContext.Rooms.Remove(entity);
            await dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task RegisterUserAsync(Room room, User user, CancellationToken cancellationToken = default)
        {
            ValidateRoom(room);
            ValidateUser(user);

            var roomEntity = await GetRoomAsync(room, cancellationToken);

            if (roomEntity is null)
            {
                throw new InvalidOperationException($"Room with id '{room.Id}' does not exist.");
            }

            UserEntity userEntity = await GetUserAsync(room, user, cancellationToken);

            // Get or update
            if (userEntity is null)
            {
                await DequeueUserAsync(room, user);

                userEntity = new UserEntity
                {
                    Id = room.Id,
                    Name = room.Name,
                    CreatedAt = DateTimeOffset.Now,
                    RoomId = roomEntity.Id,
                    Room = roomEntity
                };
            }
            else
            {
                // Already here yao
                return;
            }

            roomEntity.ChangedAt = DateTimeOffset.Now;
            await dbContext.Users.AddAsync(userEntity, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task UnregisterUserAsync(Room room, User user, CancellationToken cancellationToken = default)
        {
            UserEntity userEntity = await GetUserAsync(room, user, cancellationToken);
            if (userEntity is null)
            {
                throw new InvalidOperationException($"User with id '{user.Id}' does not exists.");
            }

            dbContext.Users.Remove(userEntity);
            await dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<bool> EnqueueUserAsync(Room room, User user, CancellationToken cancellationToken = default)
        {
            UserEntity userEntity = await GetUserAsync(room, user, cancellationToken);
            if (userEntity is null)
            {
                throw new InvalidOperationException($"User with id '{user.Id}' does not exists.");
            }

            if (userEntity.EnqueuedAt.HasValue)
            {
                return false;
            }

            userEntity.EnqueuedAt = DateTimeOffset.Now;
            await dbContext.SaveChangesAsync(cancellationToken);

            return true;
        }

        public async Task<bool> DequeueUserAsync(Room room, User user, CancellationToken cancellationToken = default)
        {
            UserEntity userEntity = await GetUserAsync(room, user, cancellationToken);
            if (userEntity is null)
            {
                throw new InvalidOperationException($"User with id '{user.Id}' does not exists.");
            }

            if (!userEntity.EnqueuedAt.HasValue)
            {
                return false;
            }

            userEntity.EnqueuedAt = null;
            await dbContext.SaveChangesAsync(cancellationToken);

            return true;
        }

        public async Task<ICollection<User>> ListIdleInRoomAsync(Room room, CancellationToken cancellationToken = default)
        {
            var userEntitiesNotInQueue = dbContext.Rooms
                .Where(o => o.Id == room.Id)
                .SelectMany(o => o.Users)
                .Where(o => !o.EnqueuedAt.HasValue);

            var usersNotInQueue = userEntitiesNotInQueue
                .Select(o => new User
                {
                    Id = o.Id,
                    Name = o.Name,
                    CreatedAt = o.CreatedAt,
                    EnqueuedAt = o.EnqueuedAt
                });

            return await usersNotInQueue.ToArrayAsync(cancellationToken);
        }

        public async Task<IList<User>> ListQueueInRoomAsync(Room room, CancellationToken cancellationToken = default)
        {
            var userEntitiesInQueue = dbContext.Rooms
                .Where(o => o.Id == room.Id)
                .SelectMany(o => o.Users)
                .Where(o => o.EnqueuedAt.HasValue);

            var usersInQueue = userEntitiesInQueue.Select(o => new User
            {
                Id = o.Id,
                Name = o.Name,
                CreatedAt = o.CreatedAt,
                EnqueuedAt = o.EnqueuedAt
            });

            return await usersInQueue.ToArrayAsync(cancellationToken);
        }

        public async Task<IList<Room>> ListRoomsAsync(CancellationToken cancellationToken = default)
        {
            var rooms = dbContext.Rooms
                .Include(o => o.Users)
                .Select(o => new Room
                {
                    Id = o.Id,
                    Name = o.Name,
                    CreatedAt = o.CreatedAt,
                    ChangedAt = o.ChangedAt,
                    Users = o.Users.Select(u => new User
                    {
                        Id = u.Id,
                        Name = u.Name,
                        CreatedAt = u.CreatedAt,
                        EnqueuedAt = u.EnqueuedAt
                    }).ToArray(),
                    Queue = o.Users
                        .Where(u => u.EnqueuedAt != null)
                        .OrderBy(u => u.EnqueuedAt)
                        .Select(u => new User
                        {
                            Id = u.Id,
                            Name = u.Name,
                            CreatedAt = u.CreatedAt,
                            EnqueuedAt = u.EnqueuedAt
                        }).ToArray()
                });

            return await rooms.ToListAsync(cancellationToken);
        }

        private Task<RoomEntity> GetRoomAsync(Room room, CancellationToken cancellationToken = default)
        {
            return dbContext.Rooms.FirstOrDefaultAsync(o => o.Id == room.Id, cancellationToken);
        }

        private Task<UserEntity> GetUserAsync(Room room, User user, CancellationToken cancellationToken = default)
        {
            return dbContext.Users.FirstOrDefaultAsync(o => o.Id == user.Id && o.RoomId == room.Id, cancellationToken);
        }

        private void ValidateRoom(Room room)
        {
            if (room.Id.IsValidIdentifier())
            {
                throw new ArgumentException($"Room property {nameof(Room.Id)} is not a valid identifier: '{room.Id}'", nameof(room));
            }

            if (room.Id.Length > RoomEntity.Id_MAX_LENGTH)
            {
                throw new ArgumentException($"Room property {nameof(Room.Id)} is too long, max is {RoomEntity.Id_MAX_LENGTH}, but got {room.Id.Length}.", nameof(room));
            }

            if (string.IsNullOrEmpty(room.Name))
            {
                throw new ArgumentException($"Room property {nameof(Room.Name)} is required.", nameof(room));
            }

            if (room.Name.Length > RoomEntity.Name_MAX_LENGTH)
            {
                throw new ArgumentException($"Room property {nameof(Room.Name)} is too long, max is {RoomEntity.Name_MAX_LENGTH}, but got {room.Name.Length}.", nameof(room));
            }
        }

        private void ValidateUser(User user)
        {
            if (user.Id.IsValidIdentifier())
            {
                throw new ArgumentException($"User property {nameof(User.Id)} is not a valid identifier: '{user.Id}'", nameof(user));
            }

            if (user.Id.Length > UserEntity.Id_MAX_LENGTH)
            {
                throw new ArgumentException($"User property {nameof(User.Id)} is too long, max is {UserEntity.Id_MAX_LENGTH}, but got {user.Id.Length}.", nameof(user));
            }

            if (string.IsNullOrEmpty(user.Name))
            {
                throw new ArgumentException($"User property {nameof(User.Name)} is required.", nameof(user));
            }

            if (user.Name.Length > UserEntity.Name_MAX_LENGTH)
            {
                throw new ArgumentException($"User property {nameof(User.Name)} is too long, max is {UserEntity.Name_MAX_LENGTH}, but got {user.Name.Length}.", nameof(user));
            }
        }
    }
}
