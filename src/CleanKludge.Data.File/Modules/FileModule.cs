using CleanKludge.Core.Articles.Data;
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
            self.TryAddSingleton<IMemoryCache>(provider => new MemoryCache(new MemoryCacheOptions()));
            self.TryAddSingleton<ISerializer, JsonSerializer>();
            self.TryAddSingleton<IArticlePath>(c => ArticlePath.For(c.GetService<IConfigurationRoot>()));
            self.TryAddSingleton<ISummaryPath>(c => SummaryPath.For(c.GetService<IConfigurationRoot>()));
            self.TryAddSingleton<IArticleSummaryRepository, ArticleSummaryRepository>();
            self.TryAddSingleton<IArticleRepository, ArticleRepository>();
            return self;
        }
    }
}