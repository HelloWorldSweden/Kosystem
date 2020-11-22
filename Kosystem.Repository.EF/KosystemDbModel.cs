using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace Kosystem.Repository.EF
{
    public class KosystemDbModel
    {
        public IModel CreateModel(DbContextOptions options)
        {
            var conventions = ConventionSet.CreateConventionSet(new KosystemDbContext(options));
            var modelBuilder = new ModelBuilder(conventions);
            OnModelCreating(modelBuilder);
            return modelBuilder.FinalizeModel();
        }

        public virtual void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DbRoom>(entity =>
            {
                entity.HasKey(o => o.Id);

                entity.HasIndex(o => o.DisplayId).IsUnique();

                entity.HasMany(o => o.People)
                    .WithOne(o => o.Room!)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            modelBuilder.Entity<DbPerson>(entity =>
            {
                entity.HasKey(o => o.Id);
            });
        }
    }
}
