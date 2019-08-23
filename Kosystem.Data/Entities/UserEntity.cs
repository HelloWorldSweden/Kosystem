﻿using Kosystem.Core;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kosystem.Data.Entities
{
    public class UserEntity
    {
        [Key]
        [StringLength(Id_MAX_LENGTH)]
        public string Id { get; set; }
        public const int Id_MAX_LENGTH = 128;

        [Required]
        [StringLength(Name_MAX_LENGTH)]
        public string Name { get; set; }
        public const int Name_MAX_LENGTH = 128;

        [Required]
        [ForeignKey(nameof(RoomId))]
        public virtual RoomEntity Room { get; set; }

        [Required]
        public string RoomId { get; set; }

        public DateTimeOffset CreatedAt { get; set; }

        public DateTimeOffset? ChangedAt { get; set; }
    }
}