using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Primitives;

namespace CleanKludge.Integration.Tests.Framework
{
    public class FakeInMemoryCache : IMemoryCache
    {
        private readonly Dictionary<object, ICacheEntry> _cache;

        public FakeInMemoryCache()
        {
            _cache = new Dictionary<object, ICacheEntry>();
        }

        public void AddValue(object key, object value)
        {
            _cache.Add(key, new FakeCacheEntry(key) { Value = value });
        }

        public void Dispose()
        {
        }

        public bool TryGetValue(object key, out object value)
        {
            var result = _cache.TryGetValue(key, out ICacheEntry cacheEntry);
            value = result ? cacheEntry.Value : null;
            return result;
        }

        public ICacheEntry CreateEntry(object key)
        {
            var fakeCacheEntry = new FakeCacheEntry(key);
            _cache[key] = fakeCacheEntry;
            return fakeCacheEntry;
        }

        public void Remove(object key)
        {
            _cache.Remove(key);
        }

        public Dictionary<string, object> GetAll()
        {
            return _cache.ToDictionary(x => x.Key.ToString(), x => x.Value.Value);
        }

        public class FakeCacheEntry : ICacheEntry
        {
            public object Key { get; }
            public object Value { get; set; }
            public DateTimeOffset? AbsoluteExpiration { get; set; }
            public TimeSpan? AbsoluteExpirationRelativeToNow { get; set; }
            public TimeSpan? SlidingExpiration { get; set; }
            public IList<IChangeToken> ExpirationTokens { get; }
            public IList<PostEvictionCallbackRegistration> PostEvictionCallbacks { get; }
            public CacheItemPriority Priority { get; set; }

            public FakeCacheEntry(object key)
            {
                Key = key;
            }

            public void Dispose()
            {
            }
        }
    }
}