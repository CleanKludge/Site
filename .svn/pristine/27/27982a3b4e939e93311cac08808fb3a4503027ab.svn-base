using System.Collections.Generic;
using System.IO;
using CleanKludge.Core.Articles;
using CleanKludge.Data.File.Errors;
using Microsoft.Extensions.Configuration;

namespace CleanKludge.Data.File.Articles
{
    public class ArticlePath
    {
        private readonly string _path;

        public static ArticlePath From(IConfigurationRoot configuration)
        {
            var path = Path.Combine(configuration["BasePath"], configuration["ArticlesPath"]);

            if (!Directory.Exists(path))
                throw ExceptionBecause.InvalidArticlePath(path);

            return new ArticlePath(path);
        }

        public ArticlePath(string path)
        {
            _path = path;
        }

        public string For(ArticleIdentifier identifier)
        {
            return Path.Combine(_path, identifier.ToString());
        }

        public IEnumerable<string> GetAll()
        {
            return Directory.GetFiles(_path, "*.json");
        }

        public string LoadFrom(ArticleIdentifier identifier)
        {
            var filePath = $"{For(identifier)}.json";

            if(!System.IO.File.Exists(filePath))
                throw ExceptionBecause.ArticleNotFound(identifier);

            return System.IO.File.ReadAllText(filePath);
        }

        public string LoadFrom(string filePath)
        {
            return System.IO.File.ReadAllText(filePath);
        }

        public void Save(ArticleIdentifier identifier, string jsonString)
        {
            System.IO.File.WriteAllText(_path, jsonString);
        }

        public void Delete(ArticleIdentifier reference)
        {
            System.IO.File.Delete(Path.Combine(_path, reference.ToString()));
        }
    }
}