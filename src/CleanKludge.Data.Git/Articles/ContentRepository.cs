using System;
using System.IO;
using LibGit2Sharp;
using Microsoft.Extensions.Options;
using Serilog;
using CleanKludge.Core.Articles;

namespace CleanKludge.Data.Git.Articles
{
    public class ContentRepository
    {
        private readonly string _contentRepositoryUri;
        private readonly GitCredentials _credentials;
        private readonly string _contentPath;
        private readonly ILogger _logger;

        public static ContentRepository For(IOptions<ContentOptions> contentOptions, IOptions<GitOptions> gitOptions, ILogger logger)
        {
            var contentOptionsValue = contentOptions.Value ?? new ContentOptions();
            var contentPath = Path.Combine(contentOptionsValue.BasePath, contentOptionsValue.ArticlesPath);

            if (!Directory.Exists(contentPath))
                Directory.CreateDirectory(contentPath);

            var gitOptionsValue = gitOptions.Value ?? new GitOptions();
            return new ContentRepository(contentPath, gitOptionsValue.ContentRepositoryUri, GitCredentials.From(gitOptionsValue), logger.ForContext<ContentRepository>());
        }

        private ContentRepository(string contentPath, string contentRepositoryUri, GitCredentials credentials, ILogger logger)
        {
            _contentPath = contentPath;
            _contentRepositoryUri = contentRepositoryUri;
            _credentials = credentials;
            _logger = logger;
        }

        public void Clone()
        {
            Repository.Clone(_contentRepositoryUri, _contentPath);
        }

        public void Pull(GitCredentials credentials)
        {
            if(!_credentials.Equals(credentials))
            {
                _logger.Error("Invalid credentials supplied for pull {@Credentials}.", credentials);
                return;
            }

            using(var repository = new Repository(_contentPath))
            {
                var pullOptions = new PullOptions();
                var signature = new Signature(credentials, DateTimeOffset.UtcNow);
                Commands.Pull(repository, signature, pullOptions);
            }
        }
    }
}