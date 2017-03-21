using System;
using CleanKludge.Core.Articles;
using CleanKludge.Core.Sections;

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

        public static Exception UnknownSectionType(SectionType sectionType)
        {
            return new ArgumentException($"Unknown section type value '{sectionType}'");
        }

        public static Exception UnknownSectionType(Api.Responses.SectionType sectionType)
        {
            return new ArgumentException($"Unknown section type value '{sectionType}'");
        }
    }
}