using CleanKludge.Core.Articles;
using Microsoft.Extensions.Caching.Memory;

namespace CleanKludge.Data.File.Articles
{
    public class ArticleRepository : IArticleRepository
    {
        private readonly IArticleSummaryRepository _summaryRepository;
        private readonly IMemoryCache _memoryCache;
        private readonly ArticlePath _articlePath;

        public ArticleRepository(ArticlePath articlePath, IMemoryCache memoryCache, IArticleSummaryRepository summaryRepository)
        {
            _articlePath = articlePath;
            _memoryCache = memoryCache;
            _summaryRepository = summaryRepository;
        }

        public IArticleDto FetchOne(ArticleIdentifier identifier)
        {
            if(_memoryCache.TryGetValue(MemoryCacheKey.ForContent(identifier), out ArticleRecord record))
                return record;

            var summary = _summaryRepository.FetchOne(identifier);
            var contents = _articlePath.LoadFor(identifier);

            record = new ArticleRecord { Summary = summary, Content = contents };
            _memoryCache.Set(MemoryCacheKey.ForContent(identifier), record);

            return record;
        }
    }
}