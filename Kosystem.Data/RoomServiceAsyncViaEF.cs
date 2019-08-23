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
            RoomEntity entity = await dbContext.Rooms.FirstOrDefaultAsync(o => o.Id == room.Id);

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

            var roomEntity = await dbContext.Rooms.FirstOrDefaultAsync(o => o.Id == room.Id, cancellationToken);

            if (roomEntity is null)
            {
                throw new InvalidOperationException($"Room with id '{room.Id}' does not exist.");
            }

            var userEntity = await dbContext.Users.FirstOrDefaultAsync(o => o.Id == user.Id, cancellationToken);
            if (userEntity.RoomId == room.Id)
            {
                // Already here yao
                return;
            }

            // Get or update
            if (userEntity is null)
            {
                await DequeueUserAsync(user);

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
                userEntity.RoomId = roomEntity.Id;
                userEntity.Room = roomEntity;
                userEntity.ChangedAt = DateTimeOffset.Now;
            }

            roomEntity.ChangedAt = DateTimeOffset.Now;
            await dbContext.Users.AddAsync(userEntity, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task UnregisterUserAsync(User user, CancellationToken cancellationToken = default)
        {
            UserEntity userEntity = await dbContext.Users.FirstOrDefaultAsync(o => o.Id == user.Id, cancellationToken);
            if (userEntity is null)
            {
                throw new InvalidOperationException($"User with id '{user.Id}' does not exists.");
            }

            dbContext.Users.Remove(userEntity);
            await dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<bool> EnqueueUserAsync(User user, CancellationToken cancellationToken = default)
        {
            UserEntity userEntity = await dbContext.Users.FirstOrDefaultAsync(o => o.Id == user.Id, cancellationToken);
            if (userEntity is null)
            {
                throw new InvalidOperationException($"User with id '{user.Id}' does not exists.");
            }

            RoomEntity roomEntity = await dbContext.Rooms
                .Include(o => o.Queue)
                .FirstOrDefaultAsync(o => o.Id == userEntity.RoomId, cancellationToken);

            if (roomEntity is null)
            {
                throw new InvalidOperationException($"User's room does not exist. Removed in parallel?");
            }

            if (await roomEntity.Queue.AnyAsync(o => o.UserId == userEntity.Id, cancellationToken))
            {
                return false;
            }

            await roomEntity.Queue.AddAsync(new UserInQueueEntity
            {
                Room = roomEntity,
                RoomId = roomEntity.Id,
                User = userEntity,
                UserId = userEntity.Id,
            }, cancellationToken);

            await dbContext.SaveChangesAsync(cancellationToken);

            return true;
        }

        public async Task<bool> DequeueUserAsync(User user, CancellationToken cancellationToken = default)
        {
            UserEntity userEntity = await dbContext.Users.FirstOrDefaultAsync(o => o.Id == user.Id, cancellationToken);
            if (userEntity is null)
            {
                throw new InvalidOperationException($"User with id '{user.Id}' does not exists.");
            }

            RoomEntity roomEntity = await dbContext.Rooms
                .Include(o => o.Queue)
                .FirstOrDefaultAsync(o => o.Id == userEntity.RoomId, cancellationToken);

            if (roomEntity is null)
            {
                throw new InvalidOperationException($"User's room does not exist. Removed in parallel?");
            }

            var userInQueue = await roomEntity.Queue.FirstOrDefaultAsync(o => o.UserId == userEntity.Id, cancellationToken);
            if (userInQueue is null)
            {
                return false;
            }

            roomEntity.Queue.Remove(userInQueue);
            await dbContext.SaveChangesAsync(cancellationToken);

            return true;
        }

        public async Task<ICollection<User>> ListIdleInRoomAsync(Room room, CancellationToken cancellationToken = default)
        {
            var usersInRoom = dbContext.Rooms
                .Where(o => o.Id == room.Id)
                .SelectMany(o => o.Users);

            var usersInQueue = dbContext.Rooms
                .Include(o => o.Queue).ThenInclude(o => o.User)
                .Where(o => o.Id == room.Id)
                .SelectMany(o => o.Queue)
                .Select(o => o.User);

            var usersNotInQueue = usersInRoom
                .Except(usersInQueue)
                .Select(o => new User
            {
                Id = o.Id,
                Name = o.Name,
                CreatedAt = o.CreatedAt,
                ChangedAt = o.ChangedAt
            });

            return await usersNotInQueue.ToArrayAsync(cancellationToken);
        }

        public async Task<IList<User>> ListQueueInRoomAsync(Room room, CancellationToken cancellationToken = default)
        {
            var userEntitiesInQueue = dbContext.Rooms
                .Include(o => o.Queue).ThenInclude(o => o.User)
                .Where(o => o.Id == room.Id)
                .SelectMany(o => o.Queue)
                .Select(o => o.User);

            var usersInQueue = userEntitiesInQueue.Select(o => new User
            {
                Id = o.Id,
                Name = o.Name,
                CreatedAt = o.CreatedAt,
                ChangedAt = o.ChangedAt
            });

            return await usersInQueue.ToArrayAsync(cancellationToken);
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
