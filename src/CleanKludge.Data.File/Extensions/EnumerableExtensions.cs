using System.Collections.Generic;
using System.Linq;

namespace CleanKludge.Data.File.Extensions
{
    public static class EnumerableExtensions
    {
        public static IList<T> ToIList<T>(this IEnumerable<T> self)
        {
            return self.ToList();
        }
    }
}