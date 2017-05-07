using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CleanKludge.Server.Authorization.Modules
{
    public static class AuthorizationModule
    {
        public static IServiceCollection AddAuthorizations(this IServiceCollection serviceCollection, IConfigurationRoot configuration)
        {
            serviceCollection.Configure<GitHubOptions>(configuration);
            return serviceCollection;
        }
    }
}