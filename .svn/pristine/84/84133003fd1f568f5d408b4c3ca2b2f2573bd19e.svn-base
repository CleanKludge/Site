using CleanKludge.Api.Responses;
using CleanKludge.Api.Responses.Articles;
using CleanKludge.Server.Articles.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CleanKludge.Server.Controllers
{
    [Route("about")]
    [AllowAnonymous]
    [DynamicArea(Location.About)]
    public class AboutController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("contact")]
        public IActionResult Contact()
        {
            return PartialView("Partials/ContactInfo");
        }
    }
}