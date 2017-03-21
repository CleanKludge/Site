using System;
using System.Collections.Generic;
using CleanKludge.Data.File.Modules;
using CleanKludge.Server.Authentication.Filters;
using CleanKludge.Server.Authentication.Modules;
using CleanKludge.Services.Modules;
using LightInject;
using LightInject.Microsoft.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Json;

namespace CleanKludge.Server
{
    public class Startup : IStartup
    {
        public IHostingEnvironment HostingEnvironment { get; }
        public IConfigurationRoot Configuration { get; }

        public Startup(IHostingEnvironment hostingEnvironment)
        {
            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .MinimumLevel.Information()
                .WriteTo.RollingFile(new JsonFormatter(), "logs/log-{Date}.log", LogEventLevel.Information)
                .CreateLogger();

            HostingEnvironment = hostingEnvironment;

            Configuration = new ConfigurationBuilder()
                .SetBasePath(hostingEnvironment.ContentRootPath)
                .AddInMemoryCollection(new [] { new KeyValuePair<string, string>("BasePath", hostingEnvironment.ContentRootPath) })
                .AddJsonFile("appsettings.json", false, true)
                .AddEnvironmentVariables()
                .Build();
        }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.TryAddSingleton(Configuration);
            services.TryAddSingleton(Log.Logger);

            services.AddOptions();
            services.AddFileServices(Configuration);
            services.AddSevices(Configuration);
            services.AddUserAuthentication();
            services.AddMvc(c => c.Filters.Add(typeof(AuthenticationAttribute)));

            return new ServiceContainer()
                .CreateServiceProvider(services);
        }

        public void Configure(IApplicationBuilder applicationBuilder)
        {
            if(HostingEnvironment.IsDevelopment())
                applicationBuilder.UseDeveloperExceptionPage();

            applicationBuilder.UseStatusCodePagesWithRedirects("/error/{0}");
            applicationBuilder.UseStaticFiles();
            applicationBuilder.AddUserAuthentication();
            applicationBuilder.UseMvc();
        }
    }
}