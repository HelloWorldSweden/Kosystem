using System.ComponentModel.DataAnnotations;

namespace Kosystem.Configuration
{
    public record LoginOptions
    {
        [Required]
        public string? Password { get; init; }
    }
}
