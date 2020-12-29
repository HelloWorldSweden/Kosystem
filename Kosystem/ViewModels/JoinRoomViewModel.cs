using System.ComponentModel.DataAnnotations;
using Kosystem.Attributes;
using Kosystem.Resources;

namespace Kosystem.ViewModels
{
    public record JoinRoomViewModel
    {
        [Display(Name = nameof(JoinRoomViewModel) + "_" + nameof(RoomId), ResourceType = typeof(DisplayNameTranslations))]
        [LocRequired]
        [LocRoomIdFormat]
        public string? RoomId { get; set; }

        [Display(Name = nameof(JoinRoomViewModel) + "_" + nameof(Name), ResourceType = typeof(DisplayNameTranslations))]
        [LocRequired]
        [LocStringMaxLength(50)]
        public string? Name { get; set; }
    }
}
