namespace CleanKludge.Core.Articles.Data
{
    public interface IArticleRepository
    {
        IArticleDto FetchOne(IArticleSummaryDto summary);
        void Reload(IArticleSummaryDto summary);
        void Remove(ArticleIdentifier identifier);
    }
}