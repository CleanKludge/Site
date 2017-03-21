using CleanKludge.Services.Authentication;
using CleanKludge.Services.Content;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace CleanKludge.Services.Modules
{
    public static class ServicesModule
    {
        public static IServiceCollection AddSevices(this IServiceCollection self, IConfigurationRoot configurationRoot)
        {  
            self.AddScoped(s => new UserService(s.GetService<IConfigurationRoot>()));
            self.TryAddTransient<ContentService>();
            return self;
        }
    }
}