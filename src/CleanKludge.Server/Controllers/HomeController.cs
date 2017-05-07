using CleanKludge.Api.Responses.Articles;
using CleanKludge.Server.Filters;
using CleanKludge.Services.Content;
using Microsoft.AspNetCore.Mvc;

namespace CleanKludge.Server.Controllers
{
    [Route("")]
    [DynamicLocation(Location.Home)]
    [ResponseCache(CacheProfileName = "Content")]
    public class HomeController : Controller
    {
        private readonly ContentService _contentService;

        public HomeController(ContentService contentService)
        {
            _contentService = contentService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var articles = _contentService.Latest();
            return View(articles.ToResponse());
        }
    }
}