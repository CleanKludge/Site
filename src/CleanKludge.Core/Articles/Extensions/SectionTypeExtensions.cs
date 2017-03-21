using CleanKludge.Core.Errors;
using CleanKludge.Core.Sections;

namespace CleanKludge.Core.Articles.Extensions
{
    public static class SectionTypeExtensions
    {
        public static Api.Responses.SectionType ToApiType(this SectionType self)
        {
            switch (self)
            {
                case SectionType.Title:
                    return Api.Responses.SectionType.Title;
                case SectionType.Code:
                    return Api.Responses.SectionType.Code;
                case SectionType.Text:
                    return Api.Responses.SectionType.Text;
                default:
                    throw ExceptionBecause.UnknownSectionType(self);
            }
        }

        public static SectionType ToCoreType(this Api.Responses.SectionType self)
        {
            switch (self)
            {
                case Api.Responses.SectionType.Title:
                    return SectionType.Title;
                case Api.Responses.SectionType.Code:
                    return SectionType.Code;
                case Api.Responses.SectionType.Text:
                    return SectionType.Text;
                default:
                    throw ExceptionBecause.UnknownSectionType(self);
            }
        }
    }
}