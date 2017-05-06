using System.Collections.Generic;

namespace CleanKludge.Core.Articles.Data
{
    public interface IArticleSummaryRepository
    {
        void Clear();
        IArticleSummaryDto FetchOne(ArticleIdentifier identifier);
        IList<IArticleSummaryDto> FetchAll();
    }
}