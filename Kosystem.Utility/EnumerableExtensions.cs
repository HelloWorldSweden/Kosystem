using System.Collections.Generic;
using System.Linq;

namespace Kosystem.Utility
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<T> WhereNotNull<T>(this IEnumerable<T?> source)
        {
            return source.Where(o => o is not null)!;
        }
    }
}
