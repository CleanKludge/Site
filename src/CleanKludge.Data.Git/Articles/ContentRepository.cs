using System;
using System.IO;
using LibGit2Sharp;
using Microsoft.Extensions.Options;
using Serilog;
using CleanKludge.Core.Articles;
using CleanKludge.Data.Git.Errors;

namespace CleanKludge.Data.Git.Articles
{
    public interface IContentRepository
    {
        void Clone();
        PullResult Pull(GitCredentials credentials);
    }

    public class ContentRepository : IContentRepository
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
            try
            {
                Repository.Clone(_contentRepositoryUri, _contentPath);
            }
            catch(Exception exception)
            {
                _logger.Error(exception, "Failed to clone repository {@ContentRepositoryUri} at {@ContentPath}.", _contentRepositoryUri, _contentPath);
            }
        }

        public PullResult Pull(GitCredentials credentials)
        {
            try
            {
                if(!_credentials.Equals(credentials))
                {
                    _logger.Error("Invalid credentials supplied for pull {Credsentials}. Wanted {RequiredCredentials}", credentials.ToString(), _credentials.ToString());
                    return PullResult.Unauthorized("Invalid credentials supplied.");
                }

                using(var repository = new Repository(_contentPath))
                {
                    var pullOptions = new PullOptions();
                    var signature = new Signature(credentials, DateTimeOffset.UtcNow);
                    var result = Commands.Pull(repository, signature, pullOptions);
                    _logger.Information("Content updated to {@Result}.", result);

                    switch(result.Status)
                    {
                        case MergeStatus.Conflicts:
                            throw ExceptionBecause.ContentHasConflicts();
                        case MergeStatus.UpToDate:
                            return PullResult.Success("Content already up to date.");
                        default:
                            return PullResult.Success($"Content updated to commit '{result.Commit?.Id?.Sha ?? "Unknown"}'.");
                    }
                }
            }
            catch(Exception exception)
            {
                _logger.Error(exception, "Pull attempted failed {@BaseException}", exception.GetBaseException());
                return PullResult.Failed("Pull attempt failed.");
            }
        }
    }
}