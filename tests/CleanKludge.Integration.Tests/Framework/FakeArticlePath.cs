using System.Collections.Generic;
using CleanKludge.Core.Articles;
using CleanKludge.Data.File.Articles;

namespace CleanKludge.Integration.Tests.Framework
{
    public class FakeArticlePath : IArticlePath
    {
        private readonly Dictionary<ArticleIdentifier, string> _articles = new Dictionary<ArticleIdentifier, string>();

        public void Add(ArticleIdentifier identifier, string content)
        {
            _articles.Add(identifier, content);
        }

        public string LoadFor(ArticleIdentifier identifier)
        {
            return _articles[identifier];
        }
    }
}