using System.Collections.Generic;
using CleanKludge.Core.Articles;
using CleanKludge.Core.Articles.Data;
using CleanKludge.Data.File.Articles;
using CleanKludge.Data.File.Serializers;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace CleanKludge.Data.File.Unit.Tests.GivenAnArticleSummaryRepository.WithAllSummariesCached
{
    [TestClass]
    public class WhenFetchingAllSummaries
    {
        private IList<IArticleSummaryDto> _result;
        private Mock<IMemoryCache> _memoryCache;
        private object _summariesCacheEntry;

        [TestInitialize]
        public void SetUp()
        {
            _summariesCacheEntry = new List<IArticleSummaryDto>
            {
                new ArticleSummaryRecord
                {
                    Identifier = ArticleIdentifier.From("article2"),
                    Title = "Title2",
                    Enabled = true
                },
                new ArticleSummaryRecord
                {
                    Identifier = ArticleIdentifier.From("article1"),
                    Title = "Title1",
                    Enabled = true
                }
            };

            _memoryCache = new Mock<IMemoryCache>();
            _memoryCache.Setup(x => x.TryGetValue("summaries", out _summariesCacheEntry)).Returns(true);

            var subject = new ArticleSummaryRepository(new Mock<IArticlePath>().Object, _memoryCache.Object, new Mock<ISerializer>().Object);
            _result = subject.FetchAll();
        }

        [TestMethod]
        public void ThenTheArticleSummarysAreReturned()
        {
            Assert.IsTrue(_result.Count == 2);
        }

        [TestMethod]
        public void ThenNothingIsAddedToTheCache()
        {
            _memoryCache.Verify(x => x.CreateEntry(It.IsAny<object>()), Times.Never);
        }
    }
}