using System;

namespace CleanKludge.Server.Extensions
{
    public static class ByteArrayExtensions
    {
        public static string ToHexString(this byte[] self)
        {
            var hex = BitConverter.ToString(self);
            return hex.Replace("-", "");
        }
    }
}