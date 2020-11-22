using System;
using System.ComponentModel.DataAnnotations;

namespace Kosystem.Repository.EF
{
    public record DbPerson
    {
        public long Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; } = string.Empty;

        public DateTime? EnqueuedAt { get; set; }

        public DbRoom? Room { get; set; }
        public int? RoomDisplayId { get; set; }
    }
}
