using CleanKludge.Api.Responses.Feed;
using CleanKludge.Services.Content;
using Microsoft.AspNetCore.Mvc;

namespace CleanKludge.Server.Controllers
{
    [Route("")]
    [ResponseCache(CacheProfileName = "None")]
    public class FeedController : Controller
    {
        private readonly ContentService _contentService;

        public FeedController(ContentService contentService)
        {
            _contentService = contentService;
        }

        [HttpGet("rss.xml")]
        [Produces("application/xml")]
        public Feed RssFeed()
        {
            return _contentService.Feed($"{Request.Scheme}://{Request.Host}");
        }
    }
}