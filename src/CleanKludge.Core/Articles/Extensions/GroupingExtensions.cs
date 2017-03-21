using CleanKludge.Api.Responses;
using CleanKludge.Api.Responses.Articles;
using CleanKludge.Core.Errors;

namespace CleanKludge.Core.Articles.Extensions
{
    public static class GroupingExtensions
    {
        public static GroupedBy ToGroupedBy(this Grouping grouping)
        {
            switch (grouping)
            {
                case Grouping.Category:
                    return GroupedBy.Category;
                case Grouping.Date:
                    return GroupedBy.Date;
                default:
                    throw ExceptionBecause.UnknownGrouping(grouping);
            }
        }
    }
}