using System.ComponentModel.DataAnnotations;

namespace Kosystem.ViewModels
{
    public class JoinRoomViewModel
    {
        [Required]
        public int? RoomId { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Name is too long.")]
        public string? Name { get; set; }
    }
}
