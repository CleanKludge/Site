using System.ComponentModel.DataAnnotations;
using CleanKludge.Api.Requests.Content;
using CleanKludge.Api.Responses.Articles;
using CleanKludge.Api.Responses.Content;
using CleanKludge.Data.Git.Articles;
using CleanKludge.Server.Authorization.Filters;
using CleanKludge.Server.Extensions;
using CleanKludge.Server.Filters;
using CleanKludge.Services.Content;
using Microsoft.AspNetCore.Mvc;

namespace CleanKludge.Server.Controllers
{
    [Route("webhook")]
    [DynamicLocation(Location.Webhook)]
    [ResponseCache(CacheProfileName = "None")]
    public class WebhookController : Controller
    {
        private readonly ContentService _contentService;

        public WebhookController(ContentService contentService)
        {
            _contentService = contentService;
        }

        [HttpPost("content")]
        [TypeFilter(typeof(ValidGitHubRequestAttribute))]
        public ContentUpdateResponse UpdateContent([FromBody] ContentUpdateRequest request)
        {
            var result = _contentService.UpdateAll(GitCredentials.From(request?.PusherRequest?.Email, request?.PusherRequest?.Name));

            Response.StatusCode = (int)result.State.ToStatusCode();
            return result.ToResponse();
        }
    }
}