using CleanKludge.Server.Authorization.Handlers;
using CleanKludge.Server.Authorization.Requirements;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CleanKludge.Server.Authorization.Modules
{
    public static class AuthorizationModule
    {
        public static IServiceCollection AddAuthorizations(this IServiceCollection serviceCollection, IConfigurationRoot configuration)
        {
            serviceCollection.Configure<GitHubOptions>(configuration);

            serviceCollection.AddAuthorization(options =>
            {
                options.AddPolicy(Policies.ValidGitHubRequest, policy => policy.Requirements.Add(new ValidGitHubRequestRequirement()));
            });

            serviceCollection.AddSingleton<IAuthorizationHandler, ValidGitHubRequestHandler>();
            return serviceCollection;
        }
    }
}