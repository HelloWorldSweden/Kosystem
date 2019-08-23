using Kosystem.Core;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kosystem.Data.Entities
{
    public class UserInQueueEntity
    {
        [Required]
        [StringLength(UserEntity.Id_MAX_LENGTH)]
        public string UserId { get; set; }

        [Required]
        [ForeignKey(nameof(UserId))]
        public virtual UserEntity User { get; set; }

        [Required]
        [StringLength(RoomEntity.Id_MAX_LENGTH)]
        public string RoomId { get; set; }

        [Required]
        [ForeignKey(nameof(RoomId))]
        public virtual RoomEntity Room { get; set; }
    }
}
