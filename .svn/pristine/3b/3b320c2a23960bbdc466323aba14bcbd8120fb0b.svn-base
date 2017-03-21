using System.Net;
using CleanKludge.Api.Responses;
using CleanKludge.Api.Responses.Articles;
using CleanKludge.Server.Articles.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace CleanKludge.Server.Controllers
{
    [Route("error")]
    [AllowAnonymous]
    [DynamicArea(Location.Error)]
    public class ErrorController : Controller
    {
        private readonly ILogger _logger;

        public ErrorController(ILogger logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Index([FromQuery(Name = "message")] string message)
        {
            return View("InternalServerError");
        }

        [HttpGet("{code}")]
        public IActionResult Index(HttpStatusCode code)
        {
            _logger.Information("[{statusCode}] {uri}", code, Request.GetDisplayUrl());

           switch (code)
           {
               case HttpStatusCode.NotFound:
                   return View("PageNotFound");
               case HttpStatusCode.Unauthorized:
                   return View("Unauthorized");
               default:
                   return View("InternalServerError");
           }
        }
    }
}