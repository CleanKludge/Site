using System;
using System.Collections.Generic;

namespace CleanKludge.Api.Responses.Articles
{
    public class ContentArticle
    {
        public string Identifier { get; set; }
        public DateTimeOffset Created { get; set; }
        public Location Location { get; set; }
        public string Author { get; set; }
        public List<string> Tags { get; set; }
        public List<ArticleSection> Sections { get; set; }
        public string Title { get; set; }
        public string Summary { get; set; }
        public bool Enabled { get; set; }

        public ContentArticle()
        {
            Created = DateTimeOffset.UtcNow;
            Sections = new List<ArticleSection>();
            Tags = new List<string>();
        }
    }
}