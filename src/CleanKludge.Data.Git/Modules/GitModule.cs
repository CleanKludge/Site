using CleanKludge.Core.Articles;
using CleanKludge.Data.Git.Articles;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Serilog;

namespace CleanKludge.Data.Git.Modules
{
    public static class GitModule
    {
        public static IServiceCollection AddNullGitServices(this IServiceCollection self, IConfigurationRoot configuration)
        {
            self.Configure<GitOptions>(configuration);
            self.TryAddSingleton<IContentRepository, NullContentRepository>();
            return self;
        }

        public static IServiceCollection AddGitServices(this IServiceCollection self, IConfigurationRoot configuration)
        {
            self.Configure<GitOptions>(configuration);
            self.TryAddSingleton<IContentRepository>(c => ContentRepository.For(c.GetService<IOptions<ContentOptions>>(), c.GetService<IOptions<GitOptions>>(), c.GetService<ILogger>()));
            return self;
        }

        public static IApplicationBuilder LoadContent(this IApplicationBuilder self)
        {
            self.ApplicationServices
                .GetService<IContentRepository>()
                ?.Clone();

            return self;
        }
    }
}