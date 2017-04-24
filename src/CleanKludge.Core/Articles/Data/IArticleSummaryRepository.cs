using System.Collections.Generic;

namespace CleanKludge.Core.Articles.Data
{
    public interface IArticleSummaryRepository
    {
        IArticleSummaryDto FetchOne(ArticleIdentifier identifier);
        IList<IArticleSummaryDto> FetchAll();
    }
}