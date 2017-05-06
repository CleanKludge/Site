using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using CleanKludge.Server.Authorization.Requirements;
using CleanKludge.Server.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Serilog;

namespace CleanKludge.Server.Authorization.Handlers
{
    public class ValidGitHubRequestHandler :  AuthorizationHandler<ValidGitHubRequestRequirement>
    {
        private const string AuthenticationType = "sha1=";
        private readonly GitHubOptions _options;
        private readonly ILogger _logger;

        public ValidGitHubRequestHandler(IOptions<GitHubOptions> options, ILogger logger)
        {
            _logger = logger.ForContext<ValidGitHubRequestHandler>();
            _options = options?.Value ?? new GitHubOptions();
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, ValidGitHubRequestRequirement requirement)
        {
            var mvcContext = context.Resource as AuthorizationFilterContext;

            if (mvcContext == null)
            {
                context.Fail();
                return;
            }

            mvcContext.HttpContext.Request.Headers.TryGetValue("X-GitHub-Event", out StringValues eventType);
            mvcContext.HttpContext.Request.Headers.TryGetValue("X-Hub-Signature", out StringValues signature);
            mvcContext.HttpContext.Request.Headers.TryGetValue("X-GitHub-Delivery", out StringValues delivery);
            var body = await mvcContext.HttpContext.Request.ReadAsByteArrayAsync();

            if(Validate(signature, eventType, delivery, body, _options.GitHubToken))
                context.Succeed(requirement);
            else
            {
                context.Fail();
                mvcContext.Result = new UnauthorizedResult();
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
            catch(Exception exception)
            {
                _logger.Error(exception, "Failed to validate {Signature} for {EventType} with {Token} on {Body}", signature, eventType, token, body);
                return false;
            }
        }
    }
}