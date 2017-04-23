using System.Collections.Generic;
using System.Linq;
using CleanKludge.Api.Responses.Articles;
using CleanKludge.Core.Articles.Extensions;

namespace CleanKludge.Core.Articles
{
    public class GroupedSummaries
    {
        private readonly Dictionary<string, List<ArticleSummary>> _articles;
        private readonly Grouping _grouping;

        public static GroupedSummaries From(List<ArticleSummary> articles, Grouping grouping)
        {
            var result = new Dictionary<string, List<ArticleSummary>>();

            foreach (var item in articles)
            {
                var keys = item.KeysFor(grouping);
                foreach(var key in keys)
                {
                    if (!result.ContainsKey(key))
                        result.Add(key, new List<ArticleSummary>());

                    result[key].Add(item);
                }
            }

            return new GroupedSummaries(result, grouping);
        }

        private GroupedSummaries(Dictionary<string, List<ArticleSummary>> articles, Grouping grouping)
        {
            _articles = articles;
            _grouping = grouping;
        }

        public GroupedSummariesResponse ToResponse()
        {
            var columns = new List<Dictionary<string, List<SummaryResponse>>>();
            foreach (var group in _articles)
            {
                if (columns.Count == 0 || columns.Last().Count == 3)
                    columns.Add(new Dictionary<string, List<SummaryResponse>>());

                columns.Last().Add(group.Key, group.Value.Select(x => x.ToResponse()).ToList());
            }

            return new GroupedSummariesResponse
            {
                GroupedBy = _grouping.ToGroupedBy(),
                Groups = columns
            };
        }
    }
}