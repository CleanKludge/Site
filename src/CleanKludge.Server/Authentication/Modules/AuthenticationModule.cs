using CleanKludge.Core.Authentication;
using CleanKludge.Server.Authentication.Stores;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace CleanKludge.Server.Authentication.Modules
{
    public static class AuthenticationModule
    {
        public static IServiceCollection AddUserAuthentication(this IServiceCollection services)
        {
            services.TryAddSingleton<IPasswordHasher<UserLogin>, PasswordHasher<UserLogin>>();
            services.AddAuthentication(options => options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme);
            return services;
        }

        public static IApplicationBuilder AddUserAuthentication(this IApplicationBuilder applicationBuilder)
        {
            applicationBuilder.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AutomaticAuthenticate = true,
                AutomaticChallenge = true,
                LoginPath = new PathString("/account/login"),
                SessionStore = new MemoryCacheTicketStore(new MemoryCache(new MemoryCacheOptions()))
            });

            return applicationBuilder;
        }
    }
}