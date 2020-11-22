using Microsoft.EntityFrameworkCore;

namespace Kosystem.Repository.EF
{
    internal class KosystemDbContext : DbContext
    {
        public KosystemDbContext(DbContextOptions options)
            : base(options)
        {

        }

        public DbSet<Person> People { get; init; } = null!;
        public DbSet<Room> Rooms { get; init; } = null!;

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
