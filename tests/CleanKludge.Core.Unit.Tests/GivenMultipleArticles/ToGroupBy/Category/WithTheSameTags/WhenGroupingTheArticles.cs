using System;
using System.Collections.Generic;
using CleanKludge.Api.Responses.Articles;
using CleanKludge.Core.Articles;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CleanKludge.Core.Unit.Tests.GivenMultipleArticles.ToGroupBy.Category.WithTheSameTags
{
    [TestClass]
    public class WhenGroupingTheArticles
    {
        private GroupedSummariesResponse _result;

        [TestInitialize]
        public void SetUp()
        {
            var articleSummaries = new List<ArticleSummary>
            {
                ArticleSummary.From(new ArticleDto { Created = new DateTimeOffset(new DateTime(2017, 01, 24, 08, 30, 55)), Title = "1", Tags = new List<string> { "1", "2" } }),
                ArticleSummary.From(new ArticleDto { Created = new DateTimeOffset(new DateTime(2017, 01, 25, 08, 30, 55)), Title = "2", Tags = new List<string> { "1", "2" } })
            };

            var subject = GroupedSummaries.From(articleSummaries, Grouping.Category);
            _result = subject.ToResponse();
        }

        [TestMethod]
        public void ThenTheArticlesAreGroupedByCategory()
        {
            Assert.IsTrue(_result.GroupedBy == GroupedBy.Category);
        }

        [TestMethod]
        public void ThenThereIsOneRow()
        {
            Assert.IsTrue(_result.Groups.Count == 1);
        }
   
        [TestMethod]
        public void ThenThereIsASingleGroup()
        {
            Assert.IsTrue(_result.Groups.Count == 1);
        }

        [TestMethod]
        public void ThenOnlyTheFirstTagIsUsed()
        {
            Assert.IsTrue(_result.Groups[0].ContainsKey("1"));
        }

        [TestMethod]
        public void ThenAllSummariesArePlacedInTheSameGroup()
        {
            Assert.IsTrue(_result.Groups[0]["1"].Count == 2);
        }
    }
}