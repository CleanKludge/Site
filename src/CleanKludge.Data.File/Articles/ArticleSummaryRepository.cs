using System.Collections.Generic;
using System.IO;
using System.Linq;
using CleanKludge.Core.Articles;
using CleanKludge.Core.Articles.Data;
using CleanKludge.Data.File.Serializers;
using Microsoft.Extensions.Caching.Memory;
using Serilog;

namespace CleanKludge.Data.File.Articles
{
    public class ArticleSummaryRepository : IArticleSummaryRepository
    {
        private readonly IArticleRepository _articleRepository;
        private readonly IMemoryCache _memoryCache;
        private readonly ISummaryPath _summaryPath;
        private readonly ISerializer _serializer;
        private readonly ILogger _logger;

        public ArticleSummaryRepository(ISummaryPath summaryPath, IMemoryCache memoryCache, ISerializer serializer, IArticleRepository articleRepository, ILogger logger)
        {
            _summaryPath = summaryPath;
            _memoryCache = memoryCache;
            _serializer = serializer;
            _articleRepository = articleRepository;
            _logger = logger.ForContext<ArticleSummaryRepository>();
        }

        public IArticleSummaryDto FetchOne(ArticleIdentifier identifier)
        {
            if(_memoryCache.TryGetValue(MemoryCacheKey.ForSummary(identifier), out ArticleSummaryRecord articleRecord))
                return articleRecord;

            var data = _summaryPath.LoadFor(identifier);
            articleRecord = _serializer.Deserialize<ArticleSummaryRecord>(data);
            return _memoryCache.Set(MemoryCacheKey.ForSummary(identifier), articleRecord);
        }

        public IList<IArticleSummaryDto> FetchAll()
        {
            if(_memoryCache.TryGetValue(MemoryCacheKey.Summaries(), out IList<IArticleSummaryDto> records))
                return records;

            records = new List<IArticleSummaryDto>();
            foreach (var path in _summaryPath.GetAll())
            {
                var fileInfo = new FileInfo(path);
                var articleIdentifier = ArticleIdentifier.From(fileInfo.Name);
                if(!_memoryCache.TryGetValue(MemoryCacheKey.ForSummary(articleIdentifier), out ArticleSummaryRecord summaryRecord))
                {
                    summaryRecord = _serializer.Deserialize<ArticleSummaryRecord>(_summaryPath.LoadFrom(path));
                    _memoryCache.Set(MemoryCacheKey.ForSummary(summaryRecord.Identifier), summaryRecord);
                }

                if (summaryRecord.Enabled)
                    records.Add(summaryRecord);
            }

            return _memoryCache
                .Set(MemoryCacheKey.Summaries(), records)
                .ToList();
        }

        public void Clear()
        {
            _memoryCache.TryGetValue(MemoryCacheKey.Summaries(), out IList<IArticleSummaryDto> oldSummaries);

            var newSummaries = new List<IArticleSummaryDto>();
            foreach(var path in _summaryPath.GetAll())
            {
                var summaryRecord = _serializer.Deserialize<ArticleSummaryRecord>(_summaryPath.LoadFrom(path));

                _memoryCache.Set(MemoryCacheKey.ForSummary(summaryRecord.Identifier), summaryRecord);
                _articleRepository.Reload(summaryRecord);
                _logger.Information("Reloaded summary '{Summary}'.", summaryRecord.Identifier.ToString());

                if (summaryRecord.Enabled)
                    newSummaries.Add(summaryRecord);
            }

            _memoryCache.Set(MemoryCacheKey.Summaries(), newSummaries);
            foreach (var oldSummary in oldSummaries ?? new List<IArticleSummaryDto>())
            {
                if(newSummaries.Any(x => x.Identifier.Equals(oldSummary.Identifier)))
                    continue;

                _memoryCache.Remove(MemoryCacheKey.ForSummary(oldSummary.Identifier));
                _articleRepository.Remove(oldSummary.Identifier);
                _logger.Information("Removed summary '{Summary}'.", oldSummary.Identifier.ToString());
            }
        }
    }
}