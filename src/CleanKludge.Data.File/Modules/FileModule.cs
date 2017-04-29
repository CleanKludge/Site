using CleanKludge.Core.Articles;
using CleanKludge.Core.Articles.Data;
using CleanKludge.Data.File.Articles;
using CleanKludge.Data.File.Serializers;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace CleanKludge.Data.File.Modules
{
    public static class FileModule
    {
        public static IServiceCollection AddFileServices(this IServiceCollection self)
        {
            self.TryAddSingleton<IMemoryCache>(provider => new MemoryCache(new MemoryCacheOptions()));
            self.TryAddSingleton<ISerializer, JsonSerializer>();
            self.TryAddSingleton<IArticlePath>(x => ArticlePath.For(x.GetService<IOptions<ContentOptions>>()));
            self.TryAddSingleton<ISummaryPath>(x => SummaryPath.For(x.GetService<IOptions<ContentOptions>>()));
            self.TryAddSingleton<IArticleSummaryRepository, ArticleSummaryRepository>();
            self.TryAddSingleton<IArticleRepository, ArticleRepository>();
            return self;
        }
    }
}