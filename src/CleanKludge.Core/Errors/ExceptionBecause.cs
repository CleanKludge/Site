using System;
using CleanKludge.Core.Articles;

namespace CleanKludge.Core.Errors
{
    public static class ExceptionBecause
    {
        public static Exception UnknownGrouping(Grouping grouping)
        {
            return new ArgumentException($"Unknown grouping value '{grouping}'");
        }

        public static Exception UnknownSection(Location location)
        {
            return new ArgumentException($"Unknown section value '{location}'");
        }

        public static Exception UnknownSection(Api.Responses.Articles.Location location)
        {
            return new ArgumentException($"Unknown section value '{location}'");
        }
    }
}