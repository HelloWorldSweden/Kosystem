using System.ComponentModel.DataAnnotations;
using Kosystem.Attributes;
using Kosystem.Resources;

namespace Kosystem.ViewModels
{
    public record LoginViewModel
    {
        [Display(Name = nameof(LoginViewModel) + "_" + nameof(Password), ResourceType = typeof(DisplayNameTranslations))]
        [LocRequired]
        public string? Password { get; set; }
    }
}
