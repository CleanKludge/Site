using CleanKludge.Core.Articles;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CleanKludge.Core.Modules
{
    public static class CoreModule
    {
        public static IServiceCollection AddCoreServices(this IServiceCollection serviceCollection, IConfigurationRoot configuration)
        {
            serviceCollection.Configure<ContentOptions>(configuration);
            return serviceCollection;
        }
    }
}