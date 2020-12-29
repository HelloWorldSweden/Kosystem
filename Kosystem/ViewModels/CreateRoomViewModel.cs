using System.ComponentModel.DataAnnotations;
using Kosystem.Attributes;
using Kosystem.Resources;

namespace Kosystem.ViewModels
{
    public record CreateRoomViewModel
    {
        [Display(Name = nameof(CreateRoomViewModel) + "_" + nameof(Name), ResourceType = typeof(DisplayNameTranslations))]
        [LocRequired]
        [LocStringMaxLength(64)]
        public string? Name { get; set; }
    }
}
