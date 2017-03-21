using CleanKludge.Core.Articles;
using CleanKludge.Data.File.Articles;
using CleanKludge.Data.File.Serializers;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace CleanKludge.Data.File.Modules
{
    public static class FileModule
    {
        public static IServiceCollection AddFileServices(this IServiceCollection self, IConfigurationRoot configuration)
        {
            self.TryAddSingleton(ArticlePath.From(configuration));
            self.TryAddSingleton<IMemoryCache>(provider => new MemoryCache(new MemoryCacheOptions()));
            self.TryAddTransient<ISerializer, JsonSerializer>();
            self.TryAddTransient<IArticleRepository, FileRepository>();
            return self;
        }
    }
}