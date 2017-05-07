using CleanKludge.Api.Responses.Articles;
using CleanKludge.Core.Articles.Data;

namespace CleanKludge.Core.Articles
{
    public class Article
    {
        private readonly ArticleSummary _summary;
        private readonly string _content;

        public static Article LoadFrom(IArticleSummaryRepository articleSummaryRepository, IArticleRepository articleRepository, ArticleIdentifier articleIdentifier)
        {
            var summary = articleSummaryRepository.FetchOne(articleIdentifier);
            var article = articleRepository.FetchOne(summary);
            return new Article(article.Content, ArticleSummary.From(summary));
        }

        private Article(string content, ArticleSummary summary)
        {
            _content = content;
            _summary = summary;
        }

        public ArticleResponse ToResponse()
        {
            return new ArticleResponse
            {
                Summary = _summary.ToResponse(),
                Content = _content
            };
        }
    }
}