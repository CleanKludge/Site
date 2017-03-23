using CleanKludge.Core.Articles;

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

        public GroupedSummaries Grouped(Grouping grouping, Location location)
        {
            return Summaries.AllFrom(_articleSummaryRepository)
                .ForSection(location)
                .GroupBy(grouping);
        }
    }
}