using CleanKludge.Core.Articles;
using CleanKludge.Core.Articles.Data;
using CleanKludge.Data.File.Articles;
using CleanKludge.Data.File.Serializers;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

using CacheEntry = CleanKludge.Data.File.Unit.Tests.Framework.CacheEntry;

namespace CleanKludge.Data.File.Unit.Tests.GivenAnArticleSummaryRepository.WithNothingCached
{
    [TestClass]
    public class WhenFetchingOneSummary
    {
        private ArticleSummaryRecord _articleSummaryRecord;
        private Mock<IMemoryCache> _memoryCache;
        private IArticleSummaryDto _result;
        private CacheEntry _cacheEntry;

        [TestInitialize]
        public void SetUp()
        {
            object cachedRecord;
            _cacheEntry = new CacheEntry();
            _memoryCache = new Mock<IMemoryCache>();
            _memoryCache.Setup(x => x.TryGetValue("summaries.identifier", out cachedRecord)).Returns(false);
            _memoryCache.Setup(x => x.CreateEntry(It.IsAny<object>())).Returns((object key) => _cacheEntry);

            var summaryPath = new Mock<ISummaryPath>();
            summaryPath.Setup(x => x.LoadFor(It.IsAny<ArticleIdentifier>())).Returns("data");

            _articleSummaryRecord = new ArticleSummaryRecord
            {
                Title = "Title"
            };

            var serializer = new Mock<ISerializer>();
            serializer.Setup(x => x.Deserialize<ArticleSummaryRecord>("data")).Returns(_articleSummaryRecord);

            var subject = new ArticleSummaryRepository(summaryPath.Object, _memoryCache.Object, serializer.Object);
            _result = subject.FetchOne(ArticleIdentifier.From("identifier"));
        }

        [TestMethod]
        public void ThenTheArticleSummaryRecordIsReturned()
        {
            Assert.IsTrue(_result.Equals(_articleSummaryRecord));
        }

        [TestMethod]
        public void ThenTheSummaryIsAddedToMemoryCache()
        {
            _memoryCache.Verify(x => x.CreateEntry("summaries.identifier"), Times.Once);
        }

        [TestMethod]
        public void ThenTheSummaryIsSetOnTheCacheEntry()
        {
            Assert.IsTrue(_cacheEntry.Value.Equals(_articleSummaryRecord));
        }
    }
}