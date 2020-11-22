using System;
using System.ComponentModel.DataAnnotations;

namespace Kosystem.Repository.EF
{
    internal record Person
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; } = string.Empty;

        public DateTime? EnqueuedAt { get; set; }

        public Room? Room { get; set; }
        public int? RoomId { get; set; }
    }
}
