using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Kosystem.Repository.EF
{
    internal record Room
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; } = string.Empty;

        public IList<Person> People { get; set; } = new List<Person>();
    }
}
