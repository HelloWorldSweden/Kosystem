using System.ComponentModel.DataAnnotations;

namespace Kosystem.ViewModels
{
    public record JoinRoomViewModel
    {
        [Required]
        public int? RoomId { get; init; }

        [Required]
        [StringLength(50, ErrorMessage = "Name is too long.")]
        public string? Name { get; init; }
    }
}
