using System.ComponentModel.DataAnnotations;

namespace Kosystem.ViewModels
{
    public record CreateRoomViewModel
    {
        [Required]
        [MaxLength(64, ErrorMessage = "Name is too long.")]
        public string? Name { get; set; }
    }
}
