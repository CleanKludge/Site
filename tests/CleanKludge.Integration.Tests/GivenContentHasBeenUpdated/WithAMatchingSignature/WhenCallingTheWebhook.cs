using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using CleanKludge.Api.Requests.Content;
using CleanKludge.Core.Articles;
using CleanKludge.Core.Articles.Data;
using CleanKludge.Data.File.Articles;
using CleanKludge.Integration.Tests.Framework;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using StoryTests.Epilogs;

namespace CleanKludge.Integration.Tests.GivenContentHasBeenUpdated.WithAMatchingSignature
{
    [TestClass]
    public class WhenCallingTheWebhook
    {
        private Dictionary<string, object> _cachedKeys;
        private HttpResponseMessage _result;

        [TestInitialize]
        public async Task SetUp()
        {
            var oldSummary = new ArticleSummaryRecord { Identifier = ArticleIdentifier.From("oldSummary"), Description = "Description", Enabled = true };
            var newSummary = new ArticleSummaryRecord { Identifier = ArticleIdentifier.From("newSummary"), Description = "Description", Enabled = true };
            var existingSummary = new ArticleSummaryRecord { Identifier = ArticleIdentifier.From("updatedSummary"), Description = "OldDescription", Enabled = true };
            var updatedSummaryRecord = new ArticleSummaryRecord { Identifier = ArticleIdentifier.From("updatedSummary"), Description = "NewDescription", Enabled = true };

            var oldArticle = new ArticleRecord { Summary = oldSummary, Content = "Content" };
            var newArticle = new ArticleRecord { Summary = newSummary, Content = "Content" };
            var existingArticle = new ArticleRecord { Summary = existingSummary, Content = "OldContent" };
            var updatedArticle = new ArticleRecord { Summary = updatedSummaryRecord, Content = "NewContent" };

            var request = new ContentUpdateRequest
            {
                PusherRequest = new PusherRequest
                {
                    Name = "name",
                    Email = "email"
                }
            };

            _result = await Given.AHttpClientWithInMemoryCache()
                .With(summaryPath => summaryPath.Add(updatedSummaryRecord))
                .With(summaryPath => summaryPath.Add(newSummary))
                .With(articlePath => articlePath.Add(updatedArticle.Summary.Identifier, updatedArticle.Content))
                .With(articlePath => articlePath.Add(newArticle.Summary.Identifier, newArticle.Content))
                .With(memoryCache => memoryCache.AddValue(MemoryCacheKey.Summaries(), new List<IArticleSummaryDto> { oldSummary, existingSummary }))
                .With(memoryCache => memoryCache.AddValue(MemoryCacheKey.ForSummary(oldSummary.Identifier), oldSummary))
                .With(memoryCache => memoryCache.AddValue(MemoryCacheKey.ForSummary(existingSummary.Identifier), existingSummary))
                .With(memoryCache => memoryCache.AddValue(MemoryCacheKey.ForContent(oldSummary.Identifier), oldArticle))
                .With(memoryCache => memoryCache.AddValue(MemoryCacheKey.ForContent(existingSummary.Identifier), existingArticle))
                .With(client => client.DefaultRequestHeaders.Add("X-GitHub-Event", "push"))
                .With(client => client.DefaultRequestHeaders.Add("X-Hub-Signature", "sha1=44d619a0e03b21f2de6517db604fcf91294240c7"))
                .With(client => client.DefaultRequestHeaders.Add("X-GitHub-Delivery", "test"))
                .When(client => client.PostAsync("webhook/content", new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json")))
                .Then((memoryCache, response) => { _cachedKeys = memoryCache.GetAll(); return Task.CompletedTask; })
                .ThenReturnTheResult();
        }

        [TestMethod]
        public void ThenTheRequestIsSuccessful()
        {
            Assert.IsTrue(_result.IsSuccessStatusCode);
        }

        [TestMethod]
        public void ThenTheOldItemsAreRemovedFromTheCache()
        {
            Assert.IsFalse(_cachedKeys.ContainsKey(MemoryCacheKey.ForSummary(ArticleIdentifier.From("oldSummary"))));
            Assert.IsFalse(_cachedKeys.ContainsKey(MemoryCacheKey.ForContent(ArticleIdentifier.From("oldSummary"))));
        }

        [TestMethod]
        public void ThenTheNewItemsAreAddedToTheCache()
        {
            Assert.IsTrue(_cachedKeys.ContainsKey(MemoryCacheKey.ForSummary(ArticleIdentifier.From("newSummary"))));
            Assert.IsTrue(_cachedKeys.ContainsKey(MemoryCacheKey.ForContent(ArticleIdentifier.From("newSummary"))));
        }

        [TestMethod]
        public void ThenTheExistingItemsAreUpdated()
        {
            var updatedSummary = (ArticleSummaryRecord)_cachedKeys[MemoryCacheKey.ForSummary(ArticleIdentifier.From("updatedSummary"))];
            Assert.IsTrue(updatedSummary.Description == "NewDescription");

            var updatedArticle = (ArticleRecord)_cachedKeys[MemoryCacheKey.ForContent(ArticleIdentifier.From("updatedSummary"))];
            Assert.IsTrue(updatedArticle.Content == "NewContent");
            Assert.IsTrue(updatedArticle.Summary.Description == "NewDescription");
        }

        [TestMethod]
        public void ThenTheActiveSummaryListIsUpdated()
        {
            var updatedSummary = (List<IArticleSummaryDto>)_cachedKeys[MemoryCacheKey.Summaries()];

            Assert.IsTrue(updatedSummary.Count == 2);
            Assert.IsTrue(updatedSummary.Count(x => x.Identifier.Equals(ArticleIdentifier.From("newSummary"))) == 1);
            Assert.IsTrue(updatedSummary.Count(x => x.Identifier.Equals(ArticleIdentifier.From("updatedSummary"))) == 1);
        }
    }
}