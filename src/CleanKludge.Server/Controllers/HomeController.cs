using CleanKludge.Api.Responses;
using CleanKludge.Api.Responses.Articles;
using CleanKludge.Server.Articles.Filters;
using CleanKludge.Services.Content;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CleanKludge.Server.Controllers
{
    [Route("")]
    [AllowAnonymous]
    [DynamicArea(Location.Home)]
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
            var articles = _contentService.Latest(User.Identity.IsAuthenticated);
            return View(articles.ToResponse());
        }
    }
}