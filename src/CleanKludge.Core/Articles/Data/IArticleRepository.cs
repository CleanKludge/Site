namespace CleanKludge.Core.Articles.Data
{
    public interface IArticleRepository
    {
        IArticleDto FetchOne(ArticleIdentifier identifier);
    }
}