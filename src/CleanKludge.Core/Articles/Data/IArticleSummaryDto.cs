using System;
using System.Collections.Generic;

namespace CleanKludge.Core.Articles
{
    public interface IArticleSummaryDto
    {
        ArticleIdentifier Identifier { get; set; }
        DateTimeOffset Created { get; set; }
        string Description { get; set; }
        string Author { get; set; }
        List<string> Tags { get; set; }
        string Title { get; set; }
        Location Location { get; set; }
        bool Enabled { get; set; }
    }
}