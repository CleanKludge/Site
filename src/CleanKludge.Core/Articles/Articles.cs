using System.Collections.Generic;
using System.Linq;
using CleanKludge.Api.Responses.Articles;

namespace CleanKludge.Core.Articles
{
    public class Articles
    {
        private readonly List<Article> _articles;

        public static Articles AllFrom(IArticleRepository contentRepository, bool includeDisabled)
        {
            return new Articles(contentRepository.FetchAll(includeDisabled).Select(Article.From).ToList());
        }

        private Articles(List<Article> articles)
        {
            _articles = articles;
        }

        public Articles GetLatest(int count)
        {
            var articles = _articles
                .OrderByDescending(x => x, new DateCreatedComparer())
                .Take(count)
                .ToList();

            return new Articles(articles);
        }

        public Articles ForSection(Location location)
        {
            return new Articles(_articles.Where(x => x.In(location)).ToList());
        }

        public GroupedArticles GroupBy(Grouping grouping)
        {
            return GroupedArticles.From(_articles, grouping);
        }

        public IList<IArticleDto> ToDto()
        {
            return _articles.Select(x => x.ToDto()).ToList();
        }

        public ContentSummaries ToResponse()
        {
            return new ContentSummaries
            {
                Summaries = _articles.Select(x => x.ToSummaryResponse()).ToList()
            };
        }
    }
}