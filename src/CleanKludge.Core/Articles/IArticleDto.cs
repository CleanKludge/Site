﻿namespace CleanKludge.Core.Articles
{
    public interface IArticleDto
    {
        IArticleSummaryDto Summary { get; set; }
        string Content { get; set; }
    }
}