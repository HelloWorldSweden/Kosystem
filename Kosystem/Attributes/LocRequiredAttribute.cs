using System.ComponentModel.DataAnnotations;
using Kosystem.Resources;

namespace Kosystem.Attributes
{
    public class LocRequiredAttribute : RequiredAttribute
    {
        public LocRequiredAttribute()
        {
            ErrorMessageResourceType = typeof(ValidationTranslations);
            ErrorMessageResourceName = nameof(ValidationTranslations.Required);
        }
    }
}
