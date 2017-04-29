using CleanKludge.Core.Articles.Data;

namespace CleanKludge.Data.File.Articles
{
    public class ArticleRecord : IArticleDto
    {
        public IArticleSummaryDto Summary { get; set; }
        public string Content { get; set; }
    }
}