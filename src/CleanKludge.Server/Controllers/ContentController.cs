using System.ComponentModel.DataAnnotations;
using CleanKludge.Api.Responses;
using CleanKludge.Api.Responses.Articles;
using CleanKludge.Core.Articles;
using CleanKludge.Server.Articles.Filters;
using CleanKludge.Server.Extensions;
using CleanKludge.Services.Content;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Location = CleanKludge.Api.Responses.Articles.Location;

namespace CleanKludge.Server.Controllers
{
    [AllowAnonymous]
    [DynamicArea(Location.Blog)]
    [Route("{location:regex(blog|experiments)}")]
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
            var articles = _contentService.Grouped(groupBy, location.ToCoreType(), User.Identity.IsAuthenticated);
            return View(articles.ToResponse());
        }

        [Authorize]
        [HttpGet("create")]
        public IActionResult Create([Required] Location location)
        {
            return View("Admin/EditArticle", new ContentArticle { Location = location });
        }

        [HttpPut("{reference}")]
        public void Put([Required] Location location, [Required] string reference, [FromBody] ContentArticle model)
        {
            _contentService.Save(Article.From(model));
        }

        [HttpDelete("{reference}")]
        [HttpPost("{reference}/delete")]
        public IActionResult Delete([Required] Location location, [Required] string reference)
        {
            _contentService.Delete(ArticleIdentifier.From(reference));
            return RedirectToAction("Posts");
        }

        [Authorize]
        [HttpGet("{reference}")]
        public IActionResult Post([Required] Location location, [Required] string reference)
        {
            var postContent = _contentService.For(ArticleIdentifier.From(reference), location.ToCoreType(), User.Identity.IsAuthenticated);
            return View(postContent.ToArticleResponse());
        }

        [Authorize]
        [HttpGet("{reference}/edit")]
        public IActionResult Edit([Required] Location location, [Required] string reference)
        {
            var postContent = _contentService.For(ArticleIdentifier.From(reference), location.ToCoreType(), User.Identity.IsAuthenticated);
            return View("Admin/EditArticle", postContent.ToArticleResponse());
        }

        [Authorize]
        [HttpPost("{reference}/section/preview")]
        public PartialViewResult PreviewSection([Required] Location location, [FromBody] ArticleSection content)
        {
            return PartialView("Admin/PreviewSection", content);
        }
    }
}