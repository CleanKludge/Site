using CleanKludge.Core.Articles;
using CleanKludge.Core.Articles.Data;
using Microsoft.Extensions.Caching.Memory;

namespace CleanKludge.Data.File.Articles
{
    public class ArticleRepository : IArticleRepository
    {
        private readonly IMemoryCache _memoryCache;
        private readonly IArticlePath _articlePath;

        public ArticleRepository(IArticlePath articlePath, IMemoryCache memoryCache)
        {
            _articlePath = articlePath;
            _memoryCache = memoryCache;
        }

        public IArticleDto FetchOne(IArticleSummaryDto summary)
        {
            if(_memoryCache.TryGetValue(MemoryCacheKey.ForContent(summary.Identifier), out ArticleRecord record))
                return record;

            var contents = _articlePath.LoadFor(summary.Identifier);

            record = new ArticleRecord { Summary = summary, Content = contents };
            _memoryCache.Set(MemoryCacheKey.ForContent(summary.Identifier), record);

            return record;
        }

        public void Reload(IArticleSummaryDto summary)
        {
            var contents = _articlePath.LoadFor(summary.Identifier);
            var record = new ArticleRecord { Summary = summary, Content = contents };
            _memoryCache.Set(MemoryCacheKey.ForContent(summary.Identifier), record);
        }

        public void Remove(ArticleIdentifier identifier)
        {
            if (!_memoryCache.TryGetValue(MemoryCacheKey.ForContent(identifier), out ArticleRecord _))
                return;

            _memoryCache.Remove(MemoryCacheKey.ForContent(identifier));
        }
    }
}