using CleanKludge.Api.Responses.Articles;
using CleanKludge.Server.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CleanKludge.Server.Controllers
{
    [Route("about")]
    [AllowAnonymous]
    [DynamicLocation(Location.About)]
    [ResponseCache(CacheProfileName = "Static")]
    public class AboutController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}