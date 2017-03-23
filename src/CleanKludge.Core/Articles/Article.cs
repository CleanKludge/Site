using CleanKludge.Api.Responses.Articles;

namespace CleanKludge.Core.Articles
{
    public class Article
    {
        private readonly ArticleSummary _summary;
        private readonly string _content;

        public static Article LoadFrom(IArticleRepository repository, ArticleIdentifier articleIdentifier)
        {
            var dto = repository.FetchOne(articleIdentifier);
            return new Article(dto.Content, ArticleSummary.From(dto.Summary));
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