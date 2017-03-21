using System.Collections.Generic;

namespace CleanKludge.Api.Responses.Articles
{
    public class ArticleSection
    {
        public SectionType Type { get; set; }
        public Dictionary<string, string> Properties { get; set; }
        public string Content { get; set; }

        public ArticleSection()
        {
            Properties = new Dictionary<string, string>();
        }
    }
}