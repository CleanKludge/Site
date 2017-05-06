using System.Runtime.CompilerServices;

namespace CleanKludge.Server.Extensions
{
    public static class StringExtensions
    {
        [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
        public static bool CryptographicEquals(this string self, string other)
        {
            if (self.Length != other.Length)
                return false;

            var result = 0;
            unchecked
            {
                for (var i = 0; i < self.Length; i++)
                    result = result | (self[i] - other[i]);
            }

            return result == 0;
        }
    }
}