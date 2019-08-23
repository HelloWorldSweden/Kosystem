using System;

namespace Kosystem.Common
{
    public static class StringHelpers
    {
        public static bool IsValidIdentifier(this string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return false;
            }

            if (!value[0].IsValidIdentifierFirstCharacter())
            {
                return false;
            }

            for (int i = 1; i < value.Length; i++)
            {
                if (!value[i].IsValidIdentifierNonFirstCharacter())
                {
                    return false;
                }
            }

            return true;
        }

        private static bool IsValidIdentifierFirstCharacter(this char value)
        {
            return (value >= 'a' && value <= 'z') ||
                (value >= 'A' && value <= 'Z') ||
                value == '_';
        }

        private static bool IsValidIdentifierNonFirstCharacter(this char value)
        {
            return (value >= 'a' && value <= 'z') ||
                (value >= 'A' && value <= 'Z') ||
                (value >= '0' && value <= '9') ||
                value == '_';
        }
    }
}
