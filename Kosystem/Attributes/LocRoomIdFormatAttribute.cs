using System.ComponentModel.DataAnnotations;
using Kosystem.Resources;

namespace Kosystem.Attributes
{
    public class LocRoomIdFormatAttribute : RegularExpressionAttribute
    {
        public LocRoomIdFormatAttribute()
            : base(@"\s*#?\s*\d{1,4}\s*")
        {
            ErrorMessageResourceType = typeof(ValidationTranslations);
            ErrorMessageResourceName = nameof(ValidationTranslations.RoomIdFormat);
        }
    }
}
