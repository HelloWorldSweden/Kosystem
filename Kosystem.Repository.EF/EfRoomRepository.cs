using System;
using System.Collections.Generic;
using System.Linq;
using Kosystem.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Kosystem.Repository.EF
{
    internal class EfRoomRepository : IRoomRepository
    {
        private readonly IDbContextFactory<KosystemDbContext> _contextFactory;
        private readonly ILogger<EfRoomRepository> _logger;
        private readonly Random _random = new Random();

        public EfRoomRepository(
            IDbContextFactory<KosystemDbContext> contextFactory,
            ILogger<EfRoomRepository> logger)
        {
            _contextFactory = contextFactory;
            _logger = logger;
        }

        public AddResult AddPersonToRoom(int roomId, long personId)
        {
            using var ctx = _contextFactory.CreateDbContext();

            var person = new DbPerson { Id = personId };
            ctx.Attach(person);
            person.RoomId = roomId;

            try
            {
                return ctx.SaveChanges() > 0
                    ? AddResult.OK
                    : AddResult.AlreadyAdded;
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Failed to assign room by ID '{roomId}' to person by ID '{personId}'.");
                return AddResult.UnableToAdd;
            }
        }

        public RoomModel CreateRoom(NewRoomModel newRoom)
        {
            const int TRIES = 30;
            var attempt = 0;
            var room = new DbRoom { Name = newRoom.Name };
            using var ctx = _contextFactory.CreateDbContext();
            ctx.Rooms.Add(room);

            while (true)
            {
                try
                {
                    lock (_random)
                    {
                        room.DisplayId = _random.Next(1, 10_000);
                    }

                    ctx.SaveChanges();
                    return room.ToRoomModel();
                }
                catch (Exception e) when (attempt < TRIES)
                {
                    _logger.LogWarning(e, $"Failed to create room by display ID '{room.DisplayId}', attempt {attempt}/{TRIES}. Will try again, it may have just been due to ID collision.");
                }
                catch (Exception e)
                {
                    _logger.LogError(e, $"Failed to create room by display ID '{room.DisplayId}', attempt {attempt}/{TRIES}. Will not try again.");
                    throw;
                }
            }
        }

        public RemoveResult DeleteRoom(int roomId)
        {
            try
            {
                using var ctx = _contextFactory.CreateDbContext();
                var room = new DbRoom { DisplayId = roomId };

                ctx.Attach(room);
                ctx.Remove(room);

                return ctx.SaveChanges() > 0
                    ? RemoveResult.OK
                    : RemoveResult.AlreadyRemoved;
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Failed to remove room by ID '{roomId}' from database.");
                return RemoveResult.UnableToRemove;
            }
        }

        public IReadOnlyCollection<PersonModel> FindPeopleInRoom(int roomId)
        {
            using var ctx = _contextFactory.CreateDbContext();
            var people = ctx.People.Where(o => o.RoomId == roomId);
            return people.Select(o => o.ToPersonModel()).ToArray();
        }

        public RoomModel? FindRoom(int roomId)
        {
            using var ctx = _contextFactory.CreateDbContext();
            return ctx.Rooms.FirstOrDefault(o => o.DisplayId == roomId)?.ToRoomModel();
        }

        public IReadOnlyCollection<RoomModel> FindRooms()
        {
            using var ctx = _contextFactory.CreateDbContext();
            return ctx.Rooms.Select(o => o.ToRoomModel()).ToArray();
        }

        public RemoveResult RemovePersonFromRoom(int roomId, long personId)
        {
            using var ctx = _contextFactory.CreateDbContext();

            var person = new DbPerson { Id = personId };
            ctx.Attach(person);
            person.RoomId = null;

            try
            {
                return ctx.SaveChanges() > 0
                    ? RemoveResult.OK
                    : RemoveResult.AlreadyRemoved;
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Failed to unassign room by ID '{roomId}' from person by ID '{personId}'.");
                return RemoveResult.UnableToRemove;
            }
        }

        public RoomModel? UpdateRoom(UpdateRoomModel patch)
        {
            using var ctx = _contextFactory.CreateDbContext();
            var room = ctx.Rooms.FirstOrDefault(o => o.DisplayId == patch.Id);

            if (room is null)
            {
                return null;
            }

            room.Name = patch.Name;
            try
            {
                ctx.SaveChanges();
                return room.ToRoomModel();
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Failed to update room by ID '{patch.Id}' in database.");
                return null;
            }
        }
    }
}
