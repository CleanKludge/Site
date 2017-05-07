using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using CleanKludge.Api.Requests.Content;
using CleanKludge.Integration.Tests.Framework;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using StoryTests.Epilogs;

namespace CleanKludge.Integration.Tests.GivenContentHasBeenUpdated.ForANonPushEvent
{
    [TestClass]
    public class WhenCallingTheWebhook
    {
        private HttpResponseMessage _result;

        [TestInitialize]
        public async Task SetUp()
        {
            var request = new ContentUpdateRequest
            {
                PusherRequest = new PusherRequest
                {
                    Name = "name",
                    Email = "email"
                }
            };

            _result = await Given.AHttpClientWithInMemoryCache()
                .With(client => client.DefaultRequestHeaders.Add("X-GitHub-Event", "pull"))
                .With(client => client.DefaultRequestHeaders.Add("X-Hub-Signature", "sha1=44d619a0e03b21f2de6517db604fcf91294240c7"))
                .With(client => client.DefaultRequestHeaders.Add("X-GitHub-Delivery", "test"))
                .When(client => client.PostAsync("webhook/content", new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json")))
                .ThenReturnTheResult();
        }

        [TestMethod]
        public void ThenTheRequestIsForbidden()
        {
            Assert.IsTrue(_result.StatusCode == HttpStatusCode.Forbidden);
        }
    }
}