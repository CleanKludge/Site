using System.Collections.Generic;

namespace CleanKludge.Core.Articles
{
    public class DateCreatedComparer : IComparer<ArticleSummary>
    {
        public int Compare(ArticleSummary x, ArticleSummary y)
        {
            return x.CompareAgeTo(y);
        }
    }
}