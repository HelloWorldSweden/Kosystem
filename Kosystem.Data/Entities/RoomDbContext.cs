using Kosystem.Common;
using Kosystem.Core;
using Kosystem.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace Kosystem.Data
{
    public class RoomDbContext : DbContext
    {
        public DbSet<UserEntity> Users { get; set; }

        public DbSet<RoomEntity> Rooms { get; set; }

        public RoomDbContext(DbContextOptions options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<UserInQueueEntity>().HasKey(table => new
            {
                table.RoomId,
                table.UserId
            });
        }

    }
}
