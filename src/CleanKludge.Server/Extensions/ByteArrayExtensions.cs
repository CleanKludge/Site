using System.Text;

namespace CleanKludge.Server.Extensions
{
    public static class ByteArrayExtensions
    {
        public static string ToHexString(this byte[] self)
        {
            var builder = new StringBuilder(self.Length * 2);
            foreach (var b in self)
                builder.AppendFormat("{0:x2}", b);

            return builder.ToString();
        }
    }
}