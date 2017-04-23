using System.Collections.Generic;
using CleanKludge.Core.Articles.Data;

namespace CleanKludge.Core.Articles
{
    public interface IArticleSummaryRepository
    {
        IArticleSummaryDto FetchOne(ArticleIdentifier identifier);
        IList<IArticleSummaryDto> FetchAll();
    }
}