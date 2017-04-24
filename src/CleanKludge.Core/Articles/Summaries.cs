using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CleanKludge.Api.Responses.Articles;
using CleanKludge.Core.Articles.Data;

namespace CleanKludge.Core.Articles
{
    public class Summaries : IEnumerable<ArticleSummary>
    {
        private readonly List<ArticleSummary> _articles;

        public static Summaries AllFrom(IArticleSummaryRepository contentRepository)
        {
            return new Summaries(contentRepository.FetchAll().Select(ArticleSummary.From).ToList());
        }

        public Summaries(List<ArticleSummary> articles)
        {
            _articles = articles;
        }

        public Summaries GetLatest(int count)
        {
            var articles = _articles
                .OrderByDescending(x => x, new DateCreatedComparer())
                .Take(count)
                .ToList();

            return new Summaries(articles);
        }

        public Summaries ForSection(Location location)
        {
            return new Summaries(_articles.Where(x => x.In(location)).ToList());
        }

        public GroupedSummaries GroupBy(Grouping grouping)
        {
            return GroupedSummaries.From(_articles, grouping);
        }

        public SummariesResponse ToResponse()
        {
            return new SummariesResponse
            {
                Summaries = _articles.Select(x => x.ToResponse()).ToList()
            };
        }

        public IEnumerator<ArticleSummary> GetEnumerator()
        {
            return _articles.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}