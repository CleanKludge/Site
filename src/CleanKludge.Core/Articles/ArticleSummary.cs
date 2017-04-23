using System.Collections.Generic;
using CleanKludge.Api.Responses.Articles;
using CleanKludge.Core.Articles.Data;
using CleanKludge.Core.Articles.Extensions;

namespace CleanKludge.Core.Articles
{
    public class ArticleSummary
    {
        private readonly IArticleSummaryDto _summary;
        
        public static ArticleSummary From(IArticleSummaryDto summary)
        {
            return new ArticleSummary(summary);
        }

        private ArticleSummary(IArticleSummaryDto summary)
        {
            _summary = summary;
        }

        public IEnumerable<string> KeysFor(Grouping grouping)
        {
            return grouping == Grouping.Date ? new List<string>{ _summary.Created.ToString("MMM yyyy") } : _summary.Tags;
        }

        public bool In(Location location)
        {
            return _summary.Location == location;
        }

        public int CompareAgeTo(ArticleSummary articleSummary)
        {
            return _summary.Created.CompareTo(articleSummary._summary.Created);
        }

        public SummaryResponse ToResponse()
        {
            return new SummaryResponse
            {
                Location =_summary.Location.ToApiType(),
                Identifier = _summary.Identifier?.ToString(),
                Title = _summary.Title,
                Created = _summary.Created,
                Description = _summary.Description,
                Enabled = _summary.Enabled,
                Tags = _summary.Tags,
                Keywords = _summary.Keywords,
                Author = _summary.Author
            };
        }
    }
}