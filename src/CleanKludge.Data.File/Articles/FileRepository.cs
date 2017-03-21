using System.Collections.Generic;
using System.IO;
using CleanKludge.Core.Articles;
using CleanKludge.Data.File.Errors;
using CleanKludge.Data.File.Serializers;
using Microsoft.Extensions.Caching.Memory;

namespace CleanKludge.Data.File.Articles
{
    public class FileRepository : IArticleRepository
    {
        private readonly IMemoryCache _memoryCache;
        private readonly ArticlePath _articlePath;
        private readonly ISerializer _serializer;

        public FileRepository(ArticlePath articlePath, IMemoryCache memoryCache, ISerializer serializer)
        {
            _articlePath = articlePath;
            _memoryCache = memoryCache;
            _serializer = serializer;
        }

        public IArticleDto FetchOne(ArticleIdentifier identifier, bool includeDisabled)
        {
            if(!_memoryCache.TryGetValue(identifier.ToString(), out ArticleRecord articleRecord))
            {
                var data = _articlePath.LoadFrom(identifier);
                articleRecord = _serializer.Deserialize<ArticleRecord>(data);
                _memoryCache.Set(identifier.ToString(), articleRecord);
            }

            if(includeDisabled || articleRecord.Enabled)
                return articleRecord.ToCoreObject();

            throw ExceptionBecause.ArticleNotFound(identifier);
        }

        public IList<IArticleDto> FetchAll(bool includeDisabled)
        {
            var dtos = new List<IArticleDto>();

            foreach (var path in _articlePath.GetAll())
            {
                var fileInfo = new FileInfo(path);

                if (!_memoryCache.TryGetValue(fileInfo.Name, out ArticleRecord articleRecord))
                {
                    articleRecord = _serializer.Deserialize<ArticleRecord>(_articlePath.LoadFrom(path));
                    _memoryCache.Set(articleRecord.Identifier, articleRecord);
                }

                if(includeDisabled || articleRecord.Enabled)
                    dtos.Add(articleRecord.ToCoreObject());
            }

            return dtos;
        }

        public void Save(IArticleDto dto)
        {
            var articleRecord = ArticleRecord.From(dto);
            _articlePath.Save(dto.Identifier, _serializer.Serialize(articleRecord));
            _memoryCache.Set(articleRecord.Identifier, articleRecord);
        }

        public void Delete(ArticleIdentifier reference)
        {
            _articlePath.Delete(reference);
            _memoryCache.Remove(reference);
        }
    }
}