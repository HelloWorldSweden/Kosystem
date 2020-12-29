using System;
using System.Text;

namespace Kosystem.Utility
{
    public static class StringHelper
    {
        public static string Capitalize(this string source)
        {
            // Don't want to use char.IsUpper here, because some
            // characters are actually neither upper nor lower (ex: '1')
            if (source.Length == 0 || !char.IsLower(source[0]))
            {
                return source;
            }

            if (source.Length < 512)
            {
                Span<char> span = stackalloc char[source.Length];
                source.AsSpan()[1..].CopyTo(span[1..]);
                span[0] = char.ToUpper(source[0]);
                return span.ToString();
            }
            else
            {
                var sb = new StringBuilder();
                sb.Append(char.ToUpper(source[0]));
                sb.Append(source, 1, source.Length - 1);
                return sb.ToString();
            }
        }
    }
}
