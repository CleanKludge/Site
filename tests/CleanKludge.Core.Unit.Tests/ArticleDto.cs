using System;
using System.Collections.Generic;
using CleanKludge.Core.Articles;

namespace CleanKludge.Core.Unit.Tests
{
    public class ArticleDto : IArticleSummaryDto
    {
        public ArticleIdentifier Identifier { get; set; }
        public DateTimeOffset Created { get; set; }
        public string Description { get; set; }
        public string Author { get; set; }
        public List<string> Tags { get; set; }
        public string Title { get; set; }
        public Location Location { get; set; }
        public bool Enabled { get; set; }
    }
}