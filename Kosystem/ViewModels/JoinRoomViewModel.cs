using System.ComponentModel.DataAnnotations;

namespace Kosystem.ViewModels
{
    public class JoinRoomViewModel
    {
        [Required]
        [RegularExpression(@"\s*#?\s*\d{1,4}\s*", ErrorMessage = "Room ID must be in the format '#1234'.")]
        public string? RoomId { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Name is too long.")]
        public string? Name { get; set; }
    }
}
