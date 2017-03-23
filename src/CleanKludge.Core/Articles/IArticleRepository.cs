namespace CleanKludge.Core.Articles
{
    public interface IArticleRepository
    {
        IArticleDto FetchOne(ArticleIdentifier identifier);
    }
}