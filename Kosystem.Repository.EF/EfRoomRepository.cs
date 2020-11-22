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

        public EfRoomRepository(
            IDbContextFactory<KosystemDbContext> contextFactory,
            ILogger<EfRoomRepository> logger)
        {
            _contextFactory = contextFactory;
            _logger = logger;
        }

        public AddResult AddPersonToRoom(int roomId, int personId)
        {
            using var ctx = _contextFactory.CreateDbContext();

            var person = new Person { Id = personId };
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
            var room = new Room { Name = newRoom.Name };
            using var ctx = _contextFactory.CreateDbContext();
            ctx.Rooms.Add(room);
            ctx.SaveChanges();
            return room.ToRoomModel();
        }

        public RemoveResult DeleteRoom(int roomId)
        {
            try
            {
                using var ctx = _contextFactory.CreateDbContext();
                var room = new Room { Id = roomId };

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
            var people = ctx.People.Where(o => o.Room?.Id == roomId);
            return people.Select(o => o.ToPersonModel()).ToArray();
        }

        public RoomModel? FindRoom(int roomId)
        {
            using var ctx = _contextFactory.CreateDbContext();
            return ctx.Rooms.FirstOrDefault(o => o.Id == roomId)?.ToRoomModel();
        }

        public IReadOnlyCollection<RoomModel> FindRooms()
        {
            using var ctx = _contextFactory.CreateDbContext();
            return ctx.Rooms.Select(o => o.ToRoomModel()).ToArray();
        }

        public RemoveResult RemovePersonFromRoom(int roomId, int personId)
        {
            using var ctx = _contextFactory.CreateDbContext();

            var person = new Person { Id = personId };
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
            var room = ctx.Rooms.FirstOrDefault(o => o.Id == patch.Id);

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
