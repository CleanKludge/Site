using CleanKludge.Core.Articles;
using CleanKludge.Data.File.Articles;
using CleanKludge.Data.File.Serializers;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace CleanKludge.Data.File.Unit.Tests.GivenAnArticleSummaryRepository.WithASummaryCached
{
    [TestClass]
    public class WhenFetchingOneSummary
    {
        private Mock<IMemoryCache> _memoryCache;
        private object _articleSummaryRecord;
        private IArticleSummaryDto _result;

        [TestInitialize]
        public void SetUp()
        {
            _articleSummaryRecord = new ArticleSummaryRecord
            {
                Title = "Title"
            };

            _memoryCache = new Mock<IMemoryCache>();
            _memoryCache.Setup(x => x.TryGetValue("summaries.identifier", out _articleSummaryRecord)).Returns(true);

            var articlePath = new Mock<IArticlePath>();
            articlePath.Setup(x => x.LoadFor(It.IsAny<ArticleIdentifier>())).Returns("data");

            var subject = new ArticleSummaryRepository(articlePath.Object, _memoryCache.Object, new Mock<ISerializer>().Object);
            _result = subject.FetchOne(ArticleIdentifier.From("identifier"));
        }

        [TestMethod]
        public void ThenTheArticleSummaryRecordIsReturned()
        {
            Assert.IsTrue(_result.Equals(_articleSummaryRecord));
        }

        [TestMethod]
        public void ThenNoItemsAreAddedToTheCache()
        {
            _memoryCache.Verify(x => x.CreateEntry(It.IsAny<object>()), Times.Never);
        }
    }
}