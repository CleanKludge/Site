using System;
using System.Collections.Generic;
using CleanKludge.Api.Responses.Articles;
using CleanKludge.Core.Articles;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CleanKludge.Core.Unit.Tests.GivenMultipleArticles.ToGroupBy.Date.WithMoreThanThreeGroups
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
                ArticleSummary.From(new ArticleDto { Created = new DateTimeOffset(new DateTime(2017, 01, 24, 08, 30, 55)), Title = "1" }),
                ArticleSummary.From(new ArticleDto { Created = new DateTimeOffset(new DateTime(2017, 02, 24, 08, 30, 55)), Title = "2" }),
                ArticleSummary.From(new ArticleDto { Created = new DateTimeOffset(new DateTime(2017, 03, 24, 08, 30, 55)), Title = "3" }),
                ArticleSummary.From(new ArticleDto { Created = new DateTimeOffset(new DateTime(2017, 04, 24, 08, 30, 55)), Title = "4" })
            };

            var subject = GroupedSummaries.From(articleSummaries, Grouping.Date);
            _result = subject.ToResponse();
        }

        [TestMethod]
        public void ThenTheArticlesAreGroupedByDate()
        {
            Assert.IsTrue(_result.GroupedBy == GroupedBy.Date);
        }

        [TestMethod]
        public void ThenThereIsTwoRows()
        {
            Assert.IsTrue(_result.Groups.Count == 2);
        }

        [TestMethod]
        public void ThenThereAreThreeGroupsInTheFirstRow()
        {
            Assert.IsTrue(_result.Groups[0].Count == 3);
        }

        [TestMethod]
        public void ThenThereIsOneGroupInTheSecondRow()
        {
            Assert.IsTrue(_result.Groups[1].Count == 1);
        }

        [TestMethod]
        public void ThenAllGroupsAreCorrect()
        {
            Assert.IsTrue(_result.Groups[0]["Jan 2017"].Count == 1);
            Assert.IsTrue(_result.Groups[0]["Feb 2017"].Count == 1);
            Assert.IsTrue(_result.Groups[0]["Mar 2017"].Count == 1);
            Assert.IsTrue(_result.Groups[1]["Apr 2017"].Count == 1);
        }
    }
}