using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using CleanKludge.Server.Authorization.Requirements;
using CleanKludge.Server.Extensions;
using Microsoft.AspNetCore.Authorization;
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
            var body = await mvcContext.HttpContext.Request.ReadAsStringAsync();

            if(Validate(signature, eventType, delivery, body, _options.GitHubToken))
                context.Succeed(requirement);
            else
                context.Fail();
        }

        public bool Validate(string signature, string eventType, string delivery, string body, string token)
        {
            try
            {
                _logger.Information("Validating {Signature} for {EventType} with {Token} on {Body}", signature ?? "null", eventType ?? "null", token ?? "null", body ?? "null");

                if (string.IsNullOrWhiteSpace(eventType) || string.IsNullOrWhiteSpace(delivery) || string.IsNullOrWhiteSpace(signature))
                    return false;

                if (!eventType.Equals("push", StringComparison.OrdinalIgnoreCase))
                    return false;

                if (!signature.StartsWith(AuthenticationType, StringComparison.OrdinalIgnoreCase))
                    return false;

                var signatureData = signature.Substring(AuthenticationType.Length);
                var secret = Encoding.ASCII.GetBytes(token);
                var payloadBytes = Encoding.ASCII.GetBytes(body);

                using (var hmacsha1 = new HMACSHA1(secret))
                {
                    var hash = hmacsha1.ComputeHash(payloadBytes);
                    var hashString = hash.ToHexString();
                    return hashString.Equals(signatureData);
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