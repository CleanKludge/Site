using System.Collections.Generic;

namespace CleanKludge.Core.Articles
{
    public interface IArticleSummaryRepository
    {
        IArticleSummaryDto FetchOne(ArticleIdentifier identifier);
        IList<IArticleSummaryDto> FetchAll();
    }
}