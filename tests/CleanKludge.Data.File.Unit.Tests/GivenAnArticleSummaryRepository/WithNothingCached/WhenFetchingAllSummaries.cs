using System.Collections.Generic;
using System.Linq;
using CleanKludge.Core.Articles;
using CleanKludge.Data.File.Articles;
using CleanKludge.Data.File.Serializers;
using CleanKludge.Data.File.Unit.Tests.Framework;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace CleanKludge.Data.File.Unit.Tests.GivenAnArticleSummaryRepository.WithNothingCached
{
    [TestClass]
    public class WhenFetchingAllSummaries
    {
        private ArticleSummaryRecord _article1SummaryRecord;
        private ArticleSummaryRecord _article2SummaryRecord;
        private IList<IArticleSummaryDto> _result;
        private Mock<IMemoryCache> _memoryCache;
        private CacheEntry _summariesCacheEntry;
        private CacheEntry _article1CacheEntry;
        private CacheEntry _article2CacheEntry;

        [TestInitialize]
        public void SetUp()
        {
            object cachedRecord;
            _summariesCacheEntry = new CacheEntry();
            _article1CacheEntry = new CacheEntry();
            _article2CacheEntry = new CacheEntry();

            _memoryCache = new Mock<IMemoryCache>();
            _memoryCache.Setup(x => x.TryGetValue("summaries", out cachedRecord)).Returns(false);
            _memoryCache.Setup(x => x.TryGetValue("summaries.article1", out cachedRecord)).Returns(false);
            _memoryCache.Setup(x => x.TryGetValue("summaries.article2", out cachedRecord)).Returns(false);
            _memoryCache.Setup(x => x.CreateEntry(It.Is<object>(y => y.Equals("summaries")))).Returns((object key) => _summariesCacheEntry);
            _memoryCache.Setup(x => x.CreateEntry(It.Is<object>(y => y.Equals("summaries.article1")))).Returns((object key) => _article1CacheEntry);
            _memoryCache.Setup(x => x.CreateEntry(It.Is<object>(y => y.Equals("summaries.article2")))).Returns((object key) => _article2CacheEntry);

            var articlePath = new Mock<IArticlePath>();
            articlePath.Setup(x => x.GetAll()).Returns(new List<string> { "article1", "article2" });
            articlePath.Setup(x => x.LoadFrom("article1")).Returns("data1");
            articlePath.Setup(x => x.LoadFrom("article2")).Returns("data2");

            _article1SummaryRecord = new ArticleSummaryRecord
            {
                Identifier = ArticleIdentifier.From("article1"),
                Title = "Title1",
                Enabled = true
            };

            _article2SummaryRecord = new ArticleSummaryRecord
            {
                Identifier = ArticleIdentifier.From("article2"),
                Title = "Title2",
                Enabled = true
            };

            var serializer = new Mock<ISerializer>();
            serializer.Setup(x => x.Deserialize<ArticleSummaryRecord>("data1")).Returns(_article1SummaryRecord);
            serializer.Setup(x => x.Deserialize<ArticleSummaryRecord>("data2")).Returns(_article2SummaryRecord);

            var subject = new ArticleSummaryRepository(articlePath.Object, _memoryCache.Object, serializer.Object);
            _result = subject.FetchAll();
        }

        [TestMethod]
        public void ThenTheArticleSummarysAreReturned()
        {
            Assert.IsTrue(_result.Count == 2);
        }

        [TestMethod]
        public void ThenBothTheSummariesAreAddedToMemoryCache()
        {
            _memoryCache.Verify(x => x.CreateEntry("summaries.article1"), Times.Once);
            Assert.IsTrue(_article1CacheEntry.Value.Equals(_article1SummaryRecord));

            _memoryCache.Verify(x => x.CreateEntry("summaries.article2"), Times.Once);
            Assert.IsTrue(_article2CacheEntry.Value.Equals(_article2SummaryRecord));
        }

        [TestMethod]
        public void ThenTheSummariesAreCached()
        {
            var articleSummaryRecords = _summariesCacheEntry.Value as IList<IArticleSummaryDto>;

            _memoryCache.Verify(x => x.CreateEntry("summaries"), Times.Once);
            Assert.IsTrue(articleSummaryRecords.SequenceEqual(new List<IArticleSummaryDto> { _article1SummaryRecord, _article2SummaryRecord } ));
        }
    }
}