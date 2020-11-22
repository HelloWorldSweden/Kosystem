using Kosystem.Repository.EF;
using Microsoft.EntityFrameworkCore;

namespace Kosystem.Data
{
    public class SqliteKosystemModel : KosystemDbModel
    {
        public override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<DbRoom>(entity =>
            {
                entity.Property(o => o.Id)
                    .ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<DbPerson>(entity =>
            {
                entity.Property(o => o.Id)
                    .ValueGeneratedOnAdd();
            });
        }
    }
}
