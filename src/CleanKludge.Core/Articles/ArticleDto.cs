using System;
using System.Collections.Generic;

namespace CleanKludge.Core.Articles
{
    public class ArticleDto : IArticleDto
    {
        public ArticleIdentifier Identifier { get; set; }
        public DateTimeOffset Created { get; set; }
        public string Description { get; set; }
        public string Author { get; set; }
        public List<string> Tags { get; set; }
        public List<ISectionDto> Sections { get; set; }
        public string Title { get; set; }
        public Location Location { get; set; }
        public bool Enabled { get; set; }
    }
}