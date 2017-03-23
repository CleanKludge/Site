using CleanKludge.Core.Articles;

namespace CleanKludge.Data.File.Articles
{
    public static class MemoryCacheKey
    {
        public static string Summaries()
        {
            return "summaries";
        }

        public static string ForSummary(ArticleIdentifier articleIdentifier)
        {
            return $"summaries.{articleIdentifier}";
        }

        public static string ForContent(ArticleIdentifier articleIdentifier)
        {
            return $"content.{articleIdentifier}";
        }
    }
}