using CleanKludge.Api.Responses.Feed;
using CleanKludge.Core.Articles;
using CleanKludge.Core.Articles.Data;
using CleanKludge.Data.Git.Articles;

namespace CleanKludge.Services.Content
{
    public class ContentService
    {
        private readonly IArticleSummaryRepository _articleSummaryRepository;
        private readonly IContentRepository _contentRepository;
        private readonly IArticleRepository _articleRepository;

        public ContentService(IArticleSummaryRepository articleSummaryRepository, IArticleRepository articleRepository, IContentRepository contentRepository)
        {
            _articleSummaryRepository = articleSummaryRepository;
            _articleRepository = articleRepository;
            _contentRepository = contentRepository;
        }

        public void UpdateAll(GitCredentials credentials)
        {
            _contentRepository.Pull(credentials);
            _articleSummaryRepository.Clear();
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