using System.Collections.Generic;

namespace CleanKludge.Core.Articles
{
    public interface IGroupedArticlesDto
    {
        Dictionary<string, List<IArticleSummaryDto>> Articles { get; set; }
        Grouping Grouping { get; set; }
    }
}