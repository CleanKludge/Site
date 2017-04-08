using System.ComponentModel.DataAnnotations;
using CleanKludge.Core.Articles;
using CleanKludge.Server.Extensions;
using CleanKludge.Server.Filters;
using CleanKludge.Services.Content;
using Microsoft.AspNetCore.Mvc;
using Location = CleanKludge.Api.Responses.Articles.Location;

namespace CleanKludge.Server.Controllers
{
    [DynamicLocation(Location.Blog)]
    [Route("{location:regex(blog|code)}")]
    public class ContentController : Controller
    {
        private readonly ContentService _contentService;

        public ContentController(ContentService contentService)
        {
            _contentService = contentService;
        }

        [HttpGet]
        public IActionResult Posts([Required] Location location, [FromQuery] Grouping groupBy = Grouping.Date)
        {
            var articles = _contentService.Grouped(groupBy, location.ToCoreType());
            return View(articles.ToResponse());
        }

        [HttpGet("{reference}")]
        public IActionResult Post([Required] Location location, [Required] string reference)
        {
            var postContent = _contentService.For(ArticleIdentifier.From(reference));
            return View(postContent.ToResponse());
        }
    }
}