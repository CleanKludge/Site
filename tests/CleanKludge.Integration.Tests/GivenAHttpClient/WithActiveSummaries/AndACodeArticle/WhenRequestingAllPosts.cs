using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using CleanKludge.Core.Articles;
using CleanKludge.Data.File.Articles;
using CleanKludge.Integration.Tests.Framework;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StoryTests.Epilogs;

namespace CleanKludge.Integration.Tests.GivenAHttpClient.WithActiveSummaries.AndACodeArticle
{
    [Ignore]
    [TestClass]
    public class WhenRequestingAllPosts
    {
        private HttpResponseMessage _result;

        [TestInitialize]
        public async Task SetUp()
        {
            var summary = new ArticleSummaryRecord
            {
                Identifier = ArticleIdentifier.From("test"),
                Description = "description",
                Author = "steve",
                Created = new DateTimeOffset(new DateTime(2014, 04, 23, 12, 33, 55)),
                Enabled = true,
                Keywords = new List<string>(),
                Location = Location.Code,
                Tags = new List<string>(),
                Title = "title"
            };

            _result = await Given.AHttpClient()
                .With(summaries => summaries.Add(summary))
                .With(articles => articles.Add(summary.Identifier, "content"))
                .When(x => x.GetAsync("code"))
                .ThenReturnTheResult();
        }

        [TestMethod]
        public void ThenTheRequestIsSuccessful()
        {
            Assert.IsTrue(_result.IsSuccessStatusCode);
        }
    }
}