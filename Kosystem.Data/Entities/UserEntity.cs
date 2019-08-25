using Kosystem.Core;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kosystem.Data.Entities
{
    public class UserEntity
    {
        public Guid Id { get; set; }

        [Required]
        [StringLength(Name_MAX_LENGTH)]
        public string Name { get; set; }
        public const int Name_MAX_LENGTH = 128;

        [Required]
        [ForeignKey(nameof(RoomId))]
        public virtual RoomEntity Room { get; set; }

        [Required]
        public Guid RoomId { get; set; }

        public DateTimeOffset CreatedAt { get; set; }

        public DateTimeOffset? EnqueuedAt { get; set; }
    }
}
