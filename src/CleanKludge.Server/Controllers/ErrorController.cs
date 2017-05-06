using System.Net;
using CleanKludge.Api.Responses.Articles;
using CleanKludge.Server.Filters;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace CleanKludge.Server.Controllers
{
    [Route("error")]
    [DynamicLocation(Location.Error)]
    public class ErrorController : Controller
    {
        private readonly ILogger _logger;

        public ErrorController(ILogger logger)
        {
            _logger = logger;
        }

        [HttpGet("")]
        public IActionResult Index()
        {
            Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            return View("InternalServerError");
        }

        [HttpGet("{code}")]
        public IActionResult Index(HttpStatusCode code)
        {
            _logger.Information("[{statusCode}] {uri}", code, Request.GetDisplayUrl());

            Response.StatusCode = (int)code;
            switch(code)
            {
                case HttpStatusCode.NotFound:
                    return View("PageNotFound");
                case HttpStatusCode.Unauthorized:
                    return View("Unauthorized");
                case HttpStatusCode.Forbidden:
                    return View("Forbidden");
                default:
                    return View("InternalServerError");
            }
        }
    }
}