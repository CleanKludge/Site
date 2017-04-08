using System;
using System.Collections.Generic;
using System.Linq;
using CleanKludge.Api.Responses.Articles;
using CleanKludge.Core.Articles;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CleanKludge.Core.Unit.Tests.GivenMultipleArticles
{
    [TestClass]
    public class WhenGettingTheLatest
    {
        private Summaries _result;

        [TestInitialize]
        public void SetUp()
        {
            var articleSummaries = new List<ArticleSummary>
            {
                ArticleSummary.From(new ArticleDto
                {
                    Created = new DateTimeOffset(new DateTime(2017, 01, 24, 08, 30, 55)),
                    Title = "1"
                }),
                ArticleSummary.From(new ArticleDto
                {
                    Created = new DateTimeOffset(new DateTime(2017, 01, 26, 08, 30, 55)),
                    Title = "2"
                }),
                ArticleSummary.From(new ArticleDto
                {
                    Created = new DateTimeOffset(new DateTime(2017, 01, 28, 08, 30, 55)),
                    Title = "3"
                }),
                ArticleSummary.From(new ArticleDto
                {
                    Created = new DateTimeOffset(new DateTime(2017, 01, 27, 08, 30, 55)),
                    Title = "4"
                }),
                ArticleSummary.From(new ArticleDto
                {
                    Created = new DateTimeOffset(new DateTime(2017, 01, 25, 08, 30, 55)),
                    Title = "5"
                }),
                ArticleSummary.From(new ArticleDto
                {
                    Created = new DateTimeOffset(new DateTime(2017, 01, 23, 08, 30, 55)),
                    Title = "6"
                })
            };

            var subject = new Summaries(articleSummaries);
            _result = subject.GetLatest(4);
        }

        [TestMethod]
        public void ThenFourArticlesAreReturned()
        {
            Assert.IsTrue(_result.Count() == 4);
        }

        [TestMethod]
        public void ThenTheLatestArticlesAreReturnedInDescendingOrder()
        {
            var articleSummaries = _result.Select(x => x.ToResponse()).ToArray();
            Assert.IsTrue(articleSummaries[0].Title == "3");
            Assert.IsTrue(articleSummaries[1].Title == "4");
            Assert.IsTrue(articleSummaries[2].Title == "2");
            Assert.IsTrue(articleSummaries[3].Title == "5");
        }
    }
}