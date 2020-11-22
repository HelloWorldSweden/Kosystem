using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Kosystem.Repository.EF
{
    public record DbRoom
    {
        public long Id { get; set; }

        public int DisplayId { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; } = string.Empty;

        public IList<DbPerson> People { get; set; } = new List<DbPerson>();
    }
}
