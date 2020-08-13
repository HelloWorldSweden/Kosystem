using System.ComponentModel.DataAnnotations;

namespace Kosystem.Configuration
{
    public class LoginOptions
    {
        [Required]
        public string? Password { get; set; }
    }
}
