using System.Collections.Generic;
using System.IO;
using System.Linq;
using CleanKludge.Core.Articles;
using CleanKludge.Data.File.Errors;
using Microsoft.Extensions.Options;

namespace CleanKludge.Data.File.Articles
{
    public interface ISummaryPath
    {
        IEnumerable<string> GetAll();
        string LoadFor(ArticleIdentifier identifier);
        string LoadFrom(string filePath);
    }

    public class SummaryPath : ISummaryPath
    {
        private readonly string _path;

        public static SummaryPath For(IOptions<ContentOptions> options)
        {
            var optionsValue = options.Value ?? new ContentOptions();
            var contentPath = Path.Combine(optionsValue.BasePath, optionsValue.ArticlesPath);

            if (!Directory.Exists(contentPath))
                throw ExceptionBecause.InvalidArticlePath(contentPath);

            return new SummaryPath(contentPath);
        }

        private SummaryPath(string path)
        {
            _path = path;
        }

        public IEnumerable<string> GetAll()
        {
            return Directory.GetFiles(_path, "*.json").Select(x => x.ToLower());
        }

        public string LoadFor(ArticleIdentifier identifier)
        {
            var filePath = $"{Path.Combine(_path, identifier.ToString())}.json";

            if (!System.IO.File.Exists(filePath))
                throw ExceptionBecause.ArticleNotFound(identifier);

            return System.IO.File.ReadAllText(filePath.ToLower());
        }

        public string LoadFrom(string filePath)
        {
            return System.IO.File.ReadAllText(filePath.ToLower());
        }
    }
}