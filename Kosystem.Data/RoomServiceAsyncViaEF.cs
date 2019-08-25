﻿using Kosystem.Common;
using Kosystem.Core;
using Kosystem.Core.DTO;
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

        public async Task<Guid> RegisterRoomAsync(RoomCreationDTO room, CancellationToken cancellationToken = default)
        {
            var entity = new RoomEntity
            {
                Id = Guid.NewGuid(),
                Name = room.Name,
                CreatedAt = DateTimeOffset.Now
            };

            await dbContext.Rooms.AddAsync(entity, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);

            return entity.Id;
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

        public async Task<Guid> RegisterUserAsync(UserCreationDTO user, CancellationToken cancellationToken = default)
        {
            var roomEntity = await GetRoomByIdAsync(user.RoomId, cancellationToken);

            if (roomEntity is null)
            {
                throw new InvalidOperationException($"Room with id '{user.RoomId}' does not exist.");
            }

            var userEntity = new UserEntity
            {
                Id = Guid.NewGuid(),
                Name = user.Name,
                CreatedAt = DateTimeOffset.Now,
                RoomId = roomEntity.Id
            };

            roomEntity.ChangedAt = DateTimeOffset.Now;
            await dbContext.Users.AddAsync(userEntity, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);

            return userEntity.Id;
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

        public async Task<Room> GetRoomByIdAsync(Guid roomId, CancellationToken cancellationToken = default)
        {
            var roomEntity = await dbContext.Rooms
                .Include(o => o.Users)
                .FirstOrDefaultAsync(o => o.Id == roomId, cancellationToken);

            if (roomEntity == null)
            {
                return null;
            }

            return new Room
            {
                Id = roomEntity.Id,
                Name = roomEntity.Name,
                CreatedAt = roomEntity.CreatedAt,
                ChangedAt = roomEntity.ChangedAt,
                Users = roomEntity.Users.Select(u => new User
                {
                    Id = u.Id,
                    Name = u.Name,
                    CreatedAt = u.CreatedAt,
                    EnqueuedAt = u.EnqueuedAt
                }).ToArray()
            };
        }

        public async Task<User> GetUserByIdAsync(Room room, Guid userId, CancellationToken cancellationToken = default)
        {
            var userEntity = await dbContext.Users
                .FirstOrDefaultAsync(o => o.Id == userId && o.RoomId == room.Id, cancellationToken);

            if (userEntity == null)
            {
                return null;
            }

            return new User
            {
                Id = userEntity.Id,
                Name = userEntity.Name,
                CreatedAt = userEntity.CreatedAt,
                EnqueuedAt = userEntity.EnqueuedAt
            };
        }
    }
}
