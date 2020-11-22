using System.ComponentModel.DataAnnotations;

namespace Kosystem.ViewModels
{
    public record LoginViewModel
    {
        [Required]
        public string? Password { get; init; }
    }
}
