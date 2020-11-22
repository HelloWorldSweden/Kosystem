using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Kosystem.Repository.EF
{
    internal class KosystemDbContext : DbContext
    {
        public KosystemDbContext(DbContextOptions options)
            : base(options)
        {
        }

        public IList<Person> People { get; init; } = new List<Person>();
        public IList<Room> Rooms { get; init; } = new List<Room>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Room>(entity =>
            {
                entity.HasKey(o => o.Id);

                entity.HasMany(o => o.People)
                    .WithOne(o => o.Room!)
                    .OnDelete(DeleteBehavior.SetNull);
            });
        }
    }
}
