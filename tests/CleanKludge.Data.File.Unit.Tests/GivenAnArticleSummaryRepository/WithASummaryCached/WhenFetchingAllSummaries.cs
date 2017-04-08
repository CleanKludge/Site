using System.Collections.Generic;
using System.Linq;
using CleanKludge.Core.Articles;
using CleanKludge.Data.File.Articles;
using CleanKludge.Data.File.Serializers;
using CleanKludge.Data.File.Unit.Tests.Framework;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace CleanKludge.Data.File.Unit.Tests.GivenAnArticleSummaryRepository.WithASummaryCached
{
    [TestClass]
    public class WhenFetchingAllSummaries
    {
        private ArticleSummaryRecord _article1SummaryRecord;
        private IList<IArticleSummaryDto> _result;
        private Mock<IMemoryCache> _memoryCache;
        private CacheEntry _summariesCacheEntry;
        private CacheEntry _article1CacheEntry;
        private object _article2SummaryRecord;

        [TestInitialize]
        public void SetUp()
        {
            object cachedRecord;
            _summariesCacheEntry = new CacheEntry();
            _article1CacheEntry = new CacheEntry();
            _article2SummaryRecord = new ArticleSummaryRecord
            {
                Identifier = ArticleIdentifier.From("article2"),
                Title = "Title2",
                Enabled = true
            };

            _article1SummaryRecord = new ArticleSummaryRecord
            {
                Identifier = ArticleIdentifier.From("article1"),
                Title = "Title1",
                Enabled = true
            };

            _memoryCache = new Mock<IMemoryCache>();
            _memoryCache.Setup(x => x.TryGetValue("summaries", out cachedRecord)).Returns(false);
            _memoryCache.Setup(x => x.TryGetValue("summaries.article1", out cachedRecord)).Returns(false);
            _memoryCache.Setup(x => x.TryGetValue("summaries.article2", out _article2SummaryRecord)).Returns(true);
            _memoryCache.Setup(x => x.CreateEntry(It.Is<object>(y => y.Equals("summaries")))).Returns((object key) => _summariesCacheEntry);
            _memoryCache.Setup(x => x.CreateEntry(It.Is<object>(y => y.Equals("summaries.article1")))).Returns((object key) => _article1CacheEntry);

            var articlePath = new Mock<IArticlePath>();
            articlePath.Setup(x => x.GetAll()).Returns(new List<string> { "article1", "article2" });
            articlePath.Setup(x => x.LoadFrom("article1")).Returns("data1");

            var serializer = new Mock<ISerializer>();
            serializer.Setup(x => x.Deserialize<ArticleSummaryRecord>("data1")).Returns(_article1SummaryRecord);

            var subject = new ArticleSummaryRepository(articlePath.Object, _memoryCache.Object, serializer.Object);
            _result = subject.FetchAll();
        }

        [TestMethod]
        public void ThenTheArticleSummarysAreReturned()
        {
            Assert.IsTrue(_result.Count == 2);
        }

        [TestMethod]
        public void ThenOnlyArticle1IsAddedToTheCache()
        {
            _memoryCache.Verify(x => x.CreateEntry("summaries.article1"), Times.Once);
            Assert.IsTrue(_article1CacheEntry.Value.Equals(_article1SummaryRecord));
        }

        [TestMethod]
        public void ThenArticle2IsNotAddedToTheCache()
        {
            _memoryCache.Verify(x => x.CreateEntry("summaries.article2"), Times.Never);
        }

        [TestMethod]
        public void ThenTheSummariesAreCached()
        {
            var articleSummaryRecords = _summariesCacheEntry.Value as IList<IArticleSummaryDto>;

            _memoryCache.Verify(x => x.CreateEntry("summaries"), Times.Once);
            Assert.IsTrue(articleSummaryRecords.SequenceEqual(new List<IArticleSummaryDto> { _article1SummaryRecord, (ArticleSummaryRecord) _article2SummaryRecord } ));
        }
    }
}