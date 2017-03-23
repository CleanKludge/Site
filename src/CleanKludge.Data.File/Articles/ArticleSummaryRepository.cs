using System.Collections.Generic;
using System.IO;
using System.Linq;
using CleanKludge.Core.Articles;
using CleanKludge.Data.File.Extensions;
using CleanKludge.Data.File.Serializers;
using Microsoft.Extensions.Caching.Memory;

namespace CleanKludge.Data.File.Articles
{
    public class ArticleSummaryRepository : IArticleSummaryRepository
    {
        private readonly IMemoryCache _memoryCache;
        private readonly ArticlePath _articlePath;
        private readonly ISerializer _serializer;

        public ArticleSummaryRepository(ArticlePath articlePath, IMemoryCache memoryCache, ISerializer serializer)
        {
            _articlePath = articlePath;
            _memoryCache = memoryCache;
            _serializer = serializer;
        }

        public IArticleSummaryDto FetchOne(ArticleIdentifier identifier)
        {
            if(_memoryCache.TryGetValue(MemoryCacheKey.ForSummary(identifier), out ArticleSummaryRecord articleRecord))
                return articleRecord;

            var data = _articlePath.LoadFor(identifier);
            articleRecord = _serializer.Deserialize<ArticleSummaryRecord>(data);
            _memoryCache.Set(MemoryCacheKey.ForContent(identifier), articleRecord);

            return articleRecord;
        }

        public IList<IArticleSummaryDto> FetchAll()
        {
            if(!_memoryCache.TryGetValue(MemoryCacheKey.Summaries(), out List<ArticleSummaryRecord> records))
            {
                records = new List<ArticleSummaryRecord>();
                foreach (var path in _articlePath.GetAll())
                {
                    var fileInfo = new FileInfo(path);
                    if(!_memoryCache.TryGetValue(fileInfo.Name, out ArticleSummaryRecord summaryRecord))
                    {
                        summaryRecord = _serializer.Deserialize<ArticleSummaryRecord>(_articlePath.LoadFrom(path));
                        _memoryCache.Set(MemoryCacheKey.ForSummary(summaryRecord.Identifier), summaryRecord);
                    }

                    if(summaryRecord.Enabled)
                        records.Add(summaryRecord);
                }
            }

            _memoryCache.Set(MemoryCacheKey.Summaries(), records);
            return records.Cast<IArticleSummaryDto>().ToIList();
        }
    }
}