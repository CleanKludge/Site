using System;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using CleanKludge.Api.Responses.Content;
using CleanKludge.Server.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Serilog;

namespace CleanKludge.Server.Authorization.Filters
{
    public class ValidGitHubRequestAttribute : Attribute, IAsyncAuthorizationFilter
    {
        private const string AuthenticationType = "sha1=";
        private readonly GitHubOptions _options;
        private readonly ILogger _logger;

        public ValidGitHubRequestAttribute(IOptions<GitHubOptions> options, ILogger logger)
        {
            _logger = logger.ForContext<ValidGitHubRequestAttribute>();
            _options = options?.Value ?? new GitHubOptions();
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            context.HttpContext.Request.Headers.TryGetValue("X-GitHub-Event", out StringValues eventType);
            context.HttpContext.Request.Headers.TryGetValue("X-Hub-Signature", out StringValues signature);
            context.HttpContext.Request.Headers.TryGetValue("X-GitHub-Delivery", out StringValues delivery);
            var body = await context.HttpContext.Request.ReadAsByteArrayAsync();

            if (!Validate(signature, eventType, delivery, body, _options.GitHubToken))
            {
                var contentUpdateResponse = new ContentUpdateResponse
                {
                    Successful = false,
                    Message = "You are not allowed to perfom that action."
                };

                context.Result = new  JsonResult(contentUpdateResponse)
                {
                    StatusCode = (int)HttpStatusCode.Forbidden
                };
            }
        }

        public bool Validate(string signature, string eventType, string delivery, byte[] body, string token)
        {
            try
            {
                _logger.Information("Validating {Signature} for {EventType} with {Token} on {Body}", signature ?? "null", eventType ?? "null", token ?? "null");

                if (string.IsNullOrWhiteSpace(eventType) || string.IsNullOrWhiteSpace(delivery) || string.IsNullOrWhiteSpace(signature) || body == null)
                    return false;

                if (!eventType.Equals("push", StringComparison.OrdinalIgnoreCase))
                    return false;

                if (!signature.StartsWith(AuthenticationType, StringComparison.OrdinalIgnoreCase))
                    return false;

                var signatureData = signature.Substring(AuthenticationType.Length).ToLower();
                using (var hmacsha1 = new HMACSHA1(Encoding.UTF8.GetBytes(token)))
                {
                    var hash = hmacsha1.ComputeHash(body);
                    var hashString = hash.ToHexString().ToLower();
                    return hashString.CryptographicEquals(signatureData);
                }
            }
            catch (Exception exception)
            {
                _logger.Error(exception, "Failed to validate {Signature} for {EventType} with {Token} on {Body}", signature, eventType, token, body);
                return false;
            }
        }
    }
}