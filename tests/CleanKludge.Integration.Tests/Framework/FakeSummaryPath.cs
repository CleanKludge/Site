using System.Collections.Generic;
using System.Linq;
using CleanKludge.Core.Articles;
using CleanKludge.Data.File.Articles;
using Newtonsoft.Json;

namespace CleanKludge.Integration.Tests.Framework
{
    public class FakeSummaryPath : ISummaryPath
    {
        private readonly Dictionary<ArticleIdentifier, string> _articles = new Dictionary<ArticleIdentifier, string>();

        public void Add(ArticleSummaryRecord summary)
        {
            _articles.Add(summary.Identifier, JsonConvert.SerializeObject(summary));
        }

        public IEnumerable<string> GetAll()
        {
            return Enumerable.Select<ArticleIdentifier, string>(_articles.Keys, x => x.ToString());
        }

        public string LoadFor(ArticleIdentifier identifier)
        {
            return _articles[identifier];
        }

        public string LoadFrom(string filePath)
        {
            return _articles[ArticleIdentifier.From(filePath)];
        }
    }
}