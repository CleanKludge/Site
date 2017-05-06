namespace CleanKludge.Data.Git.Articles
{
    public class NullContentRepository : IContentRepository
    {
        public void Clone()
        {
        }

        public void Pull(GitCredentials credentials)
        {
        }
    }
}