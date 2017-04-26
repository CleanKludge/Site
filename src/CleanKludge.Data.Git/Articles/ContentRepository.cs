using System.IO;
using LibGit2Sharp;
using Microsoft.Extensions.Configuration;

namespace CleanKludge.Data.Git.Articles
{
    public class ContentRepository
    {
        private readonly string _contentRepositoryUri;
        private readonly string _contentPath;

        public static ContentRepository For(IConfigurationRoot configuration)
        {
            var contentPath = Path.Combine(configuration["BasePath"], configuration["ArticlesPath"]);

            if (!Directory.Exists(contentPath))
                Directory.CreateDirectory(contentPath);

            return new ContentRepository(contentPath, configuration["ContentRepositoryUri"]);
        }

        private ContentRepository(string contentPath, string contentRepositoryUri)
        {
            _contentPath = contentPath;
            _contentRepositoryUri = contentRepositoryUri;
        }

        public void Clone()
        {
            Repository.Clone(_contentRepositoryUri, _contentPath);
        }
    }
}