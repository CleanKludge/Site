using CleanKludge.Data.Git.Articles;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace CleanKludge.Data.Git.Modules
{
    public static class GitModule
    {
        public static IServiceCollection AddGitServices(this IServiceCollection self, IConfigurationRoot configuration)
        {
            self.TryAddSingleton<ContentRepository>();
            return self;
        }

        public static IApplicationBuilder LoadContent(this IApplicationBuilder self)
        {
            self.ApplicationServices
                .GetService<ContentRepository>()
                ?.Clone();

            return self;
        }
    }
}