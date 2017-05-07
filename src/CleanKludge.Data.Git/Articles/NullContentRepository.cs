namespace CleanKludge.Data.Git.Articles
{
    public class NullContentRepository : IContentRepository
    {
        public void Clone()
        {
        }

        public PullResult Pull(GitCredentials credentials)
        {
            return PullResult.Success("Always succeeds");
        }
    }
}