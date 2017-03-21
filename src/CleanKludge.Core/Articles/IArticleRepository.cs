using System.Collections.Generic;

namespace CleanKludge.Core.Articles
{
    public interface IArticleRepository
    {
        IArticleDto FetchOne(ArticleIdentifier identifier, bool includeDisabled);
        IList<IArticleDto> FetchAll(bool includeDisabled);
        void Save(IArticleDto dto);
        void Delete(ArticleIdentifier reference);
    }
}