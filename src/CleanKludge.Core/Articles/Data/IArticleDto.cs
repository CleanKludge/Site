namespace CleanKludge.Core.Articles.Data
{
    public interface IArticleDto
    {
        IArticleSummaryDto Summary { get; set; }
        string Content { get; set; }
    }
}