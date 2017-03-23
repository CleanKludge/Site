using System.Collections.Generic;

namespace CleanKludge.Api.Responses.Articles
{
    public class GroupedSummariesResponse
    {
        public List<Dictionary<string, List<SummaryResponse>>> Groups { get; set; }
        public GroupedBy GroupedBy { get; set; }
    }
}