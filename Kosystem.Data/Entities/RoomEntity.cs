﻿using Kosystem.Core;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Kosystem.Data.Entities
{
    public class RoomEntity
    {
        [Key]
        [Required]
        [StringLength(Id_MAX_LENGTH)]
        public string Id { get; set; }
        public const int Id_MAX_LENGTH = 128;

        [Required]
        [StringLength(Name_MAX_LENGTH)]
        public string Name { get; set; }
        public const int Name_MAX_LENGTH = 128;

        public virtual DbSet<UserEntity> Users { get; set; }

        public virtual DbSet<UserInQueueEntity> Queue { get; set; }

        [Required]
        public DateTimeOffset CreatedAt { get; set; }

        public DateTimeOffset? ChangedAt { get; set; }
    }
}