using System.Collections.Generic;
using System.Linq;
using CleanKludge.Api.Responses;
using CleanKludge.Api.Responses.Articles;
using CleanKludge.Core.Articles.Extensions;

namespace CleanKludge.Core.Articles
{
    public class GroupedArticles
    {
        private readonly Dictionary<string, List<Article>> _articles;
        private readonly Grouping _grouping;

        public static GroupedArticles From(List<Article> articles, Grouping grouping)
        {
            var result = new Dictionary<string, List<Article>>();

            foreach (var item in articles)
            {
                var key = item.KeyFor(grouping);
                if (!result.ContainsKey(key))
                    result.Add(key, new List<Article>());

                result[key].Add(item);
            }

            return new GroupedArticles(result, grouping);
        }

        private GroupedArticles(Dictionary<string, List<Article>> articles, Grouping grouping)
        {
            _articles = articles;
            _grouping = grouping;
        }

        public IGroupedArticlesDto ToDto()
        {
            return new GroupedArticlesDto
            {
                Grouping = _grouping,
                Articles = _articles.ToDictionary(x => x.Key, x => x.Value.Select(y => y.ToDto()).ToList())
            };
        }

        public GroupedContentSummaries ToResponse()
        {
            var columns = new List<Dictionary<string, List<ContentSummary>>>();
            foreach (var group in _articles)
            {
                if (columns.Count == 0 || columns.Last().Count == 3)
                    columns.Add(new Dictionary<string, List<ContentSummary>>());

                columns.Last().Add(group.Key, group.Value.Select(x => x.ToSummaryResponse()).ToList());
            }

            return new GroupedContentSummaries
            {
                GroupedBy = _grouping.ToGroupedBy(),
                Groups = columns
            };
        }
    }
}