using Microsoft.EntityFrameworkCore;

namespace Kosystem.Repository.EF
{
    public class KosystemDbContext : DbContext
    {
        public KosystemDbContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<DbPerson> People { get; init; } = null!;
        public DbSet<DbRoom> Rooms { get; init; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // This behaviour can be overridden by setting
            // DbContextOptions.Model = new KosystemDbModel().CreateModel()
            new KosystemDbModel().OnModelCreating(modelBuilder);
        }
    }
}
