using System.Collections.Generic;
using System.IO;
using CleanKludge.Core.Articles;
using CleanKludge.Data.File.Errors;
using Microsoft.Extensions.Configuration;

namespace CleanKludge.Data.File.Articles
{
    public interface IArticlePath
    {
        IEnumerable<string> GetAll();
        string LoadFor(ArticleIdentifier identifier);
        string LoadFrom(string filePath);
    }

    public class ArticlePath : IArticlePath
    {
        private readonly string _extension;
        private readonly string _path;

        public static ArticlePath ForSummaries(IConfigurationRoot configuration)
        {
            var contentPath = Path.Combine(configuration["BasePath"], configuration["ArticlesPath"]);

            if (!Directory.Exists(contentPath))
                throw ExceptionBecause.InvalidArticlePath(contentPath);

            return new ArticlePath(contentPath, ".json");
        }

        public static ArticlePath ForContent(IConfigurationRoot configuration)
        {
            var contentPath = Path.Combine(configuration["BasePath"], configuration["ArticlesPath"]);

            if (!Directory.Exists(contentPath))
                throw ExceptionBecause.InvalidArticlePath(contentPath);

            return new ArticlePath(contentPath, ".md");
        }

        public ArticlePath(string path, string extension)
        {
            _path = path;
            _extension = extension;
        }

        public IEnumerable<string> GetAll()
        {
            return Directory.GetFiles(_path, $"*{_extension}");
        }

        public string LoadFor(ArticleIdentifier identifier)
        {
            var filePath = $"{Path.Combine(_path, identifier.ToString())}{_extension}";

            if(!System.IO.File.Exists(filePath))
                throw ExceptionBecause.ArticleNotFound(identifier);

            return System.IO.File.ReadAllText(filePath);
        }

        public string LoadFrom(string filePath)
        {
            return System.IO.File.ReadAllText(filePath);
        }
    }
}