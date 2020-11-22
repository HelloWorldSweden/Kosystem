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
                entity.HasKey(o => o.Id);

                entity.Property(o => o.Id)
                    .HasColumnType("INTEGER PRIMARY KEY")
                    .ValueGeneratedNever();
            });

            modelBuilder.Entity<DbPerson>(entity =>
            {
                entity.HasKey(o => o.Id);

                entity.Property(o => o.Id)
                    .HasColumnType("INTEGER PRIMARY KEY")
                    .ValueGeneratedNever();

                entity.Property(o => o.RoomId)
                    .HasColumnType("INTEGER");
            });
        }
    }
}
