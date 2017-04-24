using System.Collections.Generic;

namespace CleanKludge.Core.Articles.Data
{
    public interface IGroupedArticlesDto
    {
        Dictionary<string, List<IArticleSummaryDto>> Articles { get; set; }
        Grouping Grouping { get; set; }
    }
}