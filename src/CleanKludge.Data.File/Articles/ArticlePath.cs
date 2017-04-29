using System.IO;
using CleanKludge.Core.Articles;
using CleanKludge.Data.File.Errors;
using Microsoft.Extensions.Options;

namespace CleanKludge.Data.File.Articles
{
    public interface IArticlePath
    {
        string LoadFor(ArticleIdentifier identifier);
    }

    public class ArticlePath : IArticlePath
    {
        private readonly string _path;

        public static ArticlePath For(IOptions<ContentOptions> options)
        {
            var optionsValue = options.Value ?? new ContentOptions();
            var contentPath = Path.Combine(optionsValue.BasePath, optionsValue.ArticlesPath);

            if (!Directory.Exists(contentPath))
                throw ExceptionBecause.InvalidArticlePath(contentPath);

            return new ArticlePath(contentPath);
        }

        private ArticlePath(string path)
        {
            _path = path;
        }

        public string LoadFor(ArticleIdentifier identifier)
        {
            var filePath = $"{Path.Combine(_path, identifier.ToString())}.md";

            if(!System.IO.File.Exists(filePath))
                throw ExceptionBecause.ArticleNotFound(identifier);

            return System.IO.File.ReadAllText(filePath.ToLower());
        }
    }
}