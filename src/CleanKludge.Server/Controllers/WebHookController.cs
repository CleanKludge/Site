using System.ComponentModel.DataAnnotations;
using CleanKludge.Api.Requests.Content;
using CleanKludge.Api.Responses.Articles;
using CleanKludge.Data.Git.Articles;
using CleanKludge.Server.Authorization;
using CleanKludge.Server.Filters;
using CleanKludge.Services.Content;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CleanKludge.Server.Controllers
{
    [Route("webhook")]
    [DynamicLocation(Location.Webhook)]
    public class WebHookController : Controller
    {
        private readonly ContentService _contentService;

        public WebHookController(ContentService contentService)
        {
            _contentService = contentService;
        }

        [HttpPost("content")]
        [Authorize(Policy = Policies.ValidGitHubRequest)]
        public void UpdateContent([FromBody, Required] ContentUpdateRequest request)
        {
            _contentService.UpdateAll(GitCredentials.From(request?.PusherRequest?.Name, request?.PusherRequest?.Email));
        }
    }
}