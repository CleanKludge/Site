using System.Collections.Generic;

namespace CleanKludge.Core.Articles
{
    public class DateCreatedComparer : IComparer<Article>
    {
        public int Compare(Article x, Article y)
        {
            return x.CompareAgeTo(y);
        }
    }
}