using System.Collections.Generic;

namespace CleanKludge.Api.Responses.Articles
{
    public class GroupedContentSummaries
    {
        public List<Dictionary<string, List<ContentSummary>>> Groups { get; set; }
        public GroupedBy GroupedBy { get; set; }
    }
}