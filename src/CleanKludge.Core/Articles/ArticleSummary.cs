using CleanKludge.Api.Responses.Articles;

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

        public string KeyFor(Grouping grouping)
        {
            return grouping == Grouping.Date ? _summary.Created.ToString("MMM yyyy") : _summary.Tags[0];
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
                Area =_summary.Location.ToString(),
                Identifier = _summary.Identifier.ToString(),
                Title = _summary.Title,
                Created = _summary.Created,
                Description = _summary.Description,
                Enabled = _summary.Enabled,
                Tags = _summary.Tags,
                Author = _summary.Author
            };
        }
    }
}