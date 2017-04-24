using System.Net.Http;
using System.Threading.Tasks;
using CleanKludge.Integration.Tests.Framework;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StoryTests.Epilogs;

namespace CleanKludge.Integration.Tests.GivenAHttpClient
{
    [Ignore]
    [TestClass]
    public class WhenRequestingTheAboutPage
    {
        private HttpResponseMessage _result;

        [TestInitialize]
        public async Task SetUp()
        {
            _result = await Given.AHttpClient()
                .When(x => x.GetAsync("about"))
                .ThenReturnTheResult();
        }

        [TestMethod]
        public void ThenTheRequestIsSuccessful()
        {
            Assert.IsTrue(_result.IsSuccessStatusCode);
        }
    }
}