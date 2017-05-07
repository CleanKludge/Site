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

        public PullResult UpdateAll(GitCredentials credentials)
        {
            var result = _contentRepository.Pull(credentials);

            if(result.State == PullState.Successful)
                _articleSummaryRepository.Clear();

            return result;
        }

        public Article For(ArticleIdentifier reference)
        {
            return Article.LoadFrom(_articleSummaryRepository, _articleRepository, reference);
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