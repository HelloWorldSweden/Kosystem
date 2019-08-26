using JetBrains.Annotations;
using Kosystem.Core;
using Kosystem.Core.DTO;
using Kosystem.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Kosystem.Data.Tests
{
    public class RegisteredRoomTests
    {
        private static DbContextOptions GetInMemoryDbOptions(string name)
        {
            return new DbContextOptionsBuilder()
                .UseInMemoryDatabase(name)
                .Options;
        }

        [Fact]
        public async Task RegisterWritesToDb()
        {
            // Arrange
            var options = GetInMemoryDbOptions("RegisterWritesToDb");
            using (var context = new RoomDbContext(options))
            {
                Assert.Equal(0, context.Rooms.Count());
            }

            // Act
            using (var context = new RoomDbContext(options))
            {
                IRoomService service = new RoomServiceAsyncViaEF(context);
                await service.RegisterRoomAsync(new RoomCreationDTO
                {
                    Name = "Foo"
                });
            }

            // Assert
            using (var context = new RoomDbContext(options))
            {
                Assert.Equal(1, context.Rooms.Count());

                var room = context.Rooms.First();
                Assert.Equal("Foo", room.Name);

                Assert.NotEqual(Guid.Empty, room.Id);
                Assert.NotEqual(default, room.CreatedAt);
                Assert.Null(room.ChangedAt);
            }
        }

        [Fact]
        public async Task UnregisterRemovesFromDb()
        {
            // Arrange
            var options = GetInMemoryDbOptions("UnregisterRemovesFromDb");
            var id = Guid.Parse("2bedb00a-d0e9-4cb3-aec3-4f8a9661e421");
            using (var context = new RoomDbContext(options))
            {
                context.Rooms.Add(new RoomEntity
                {
                    Name = "Foo",
                    Id = id
                });
                context.SaveChanges();
                Assert.Equal(1, context.Rooms.Count());
            }

            // Act
            using (var context = new RoomDbContext(options))
            {
                IRoomService service = new RoomServiceAsyncViaEF(context);
                await service.UnregisterRoomAsync(id);
            }

            // Assert
            using (var context = new RoomDbContext(options))
            {
                Assert.Equal(0, context.Rooms.Count());
            }
        }
    }
}
