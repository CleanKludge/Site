using System.ComponentModel.DataAnnotations;
using CleanKludge.Api.Requests.Content;
using CleanKludge.Data.File.Articles;
using CleanKludge.Data.Git.Articles;
using CleanKludge.Server.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CleanKludge.Server.Controllers
{
    [Route("webhook")]
    public class WebHookController : Controller
    {
        private readonly ArticleSummaryRepository _articleSummaryRepository;
        private readonly ContentRepository _contentRepository;

        public WebHookController(ContentRepository contentRepository, ArticleSummaryRepository articleSummaryRepository)
        {
            _contentRepository = contentRepository;
            _articleSummaryRepository = articleSummaryRepository;
        }

        [HttpPost("content")]
        [Authorize(Policy = Policies.ValidGitHubRequest)]
        public void UpdateContent([FromBody, Required] ContentUpdateRequest request)
        {
            _contentRepository.Pull(GitCredentials.From(request?.PusherRequest?.Name, request?.PusherRequest?.Email));
            _articleSummaryRepository.Clear();
        }
    }
}