using CleanKludge.Api.Responses.Feed;
using CleanKludge.Core.Articles;
using CleanKludge.Core.Articles.Data;

namespace CleanKludge.Services.Content
{
    public class ContentService
    {
        private readonly IArticleSummaryRepository _articleSummaryRepository;
        private readonly IArticleRepository _articleRepository;

        public ContentService(IArticleSummaryRepository articleSummaryRepository, IArticleRepository articleRepository)
        {
            _articleSummaryRepository = articleSummaryRepository;
            _articleRepository = articleRepository;
        }

        public Article For(ArticleIdentifier reference)
        {
            return Article.LoadFrom(_articleRepository, reference);
        }

        public Summaries Latest()
        {
            return Summaries.AllFrom(_articleSummaryRepository)
                .GetLatest(5);
        }

        public Feed Feed(string serverUri)
        {
            return Summaries.AllFrom(_articleSummaryRepository)
                .ToFeed(serverUri);
        }

        public GroupedSummaries Grouped(Grouping grouping, Location location)
        {
            return Summaries.AllFrom(_articleSummaryRepository)
                .ForSection(location)
                .GroupBy(grouping);
        }
    }
}