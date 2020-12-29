using System.ComponentModel.DataAnnotations;
using Kosystem.Resources;

namespace Kosystem.Attributes
{
    public class LocStringMaxLengthAttribute : StringLengthAttribute
    {
        public LocStringMaxLengthAttribute(int maximumLength)
            : base(maximumLength)
        {
            ErrorMessageResourceType = typeof(ValidationTranslations);
            ErrorMessageResourceName = nameof(ValidationTranslations.StringMaxLength);
        }
    }
}
