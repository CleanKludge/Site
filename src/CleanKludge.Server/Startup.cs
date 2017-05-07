using System;
using System.Buffers;
using System.Collections.Generic;
using System.Xml;
using CleanKludge.Core.Modules;
using CleanKludge.Data.File.Modules;
using CleanKludge.Data.Git.Modules;
using CleanKludge.Server.Authorization.Modules;
using CleanKludge.Server.Filters;
using CleanKludge.Services.Modules;
using LightInject;
using LightInject.Microsoft.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Json;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;

namespace CleanKludge.Server
{
    public class Startup : IStartup
    {
        public IHostingEnvironment HostingEnvironment { get; }
        public IConfigurationRoot Configuration { get; }

        public Startup(IHostingEnvironment hostingEnvironment, ILoggerFactory loggerFactory)
        {
            HostingEnvironment = hostingEnvironment;

            Configuration = new ConfigurationBuilder()
                .SetBasePath(hostingEnvironment.ContentRootPath)
                .AddInMemoryCollection(new[] { new KeyValuePair<string, string>("BasePath", hostingEnvironment.ContentRootPath) })
                .AddJsonFile("appsettings.json", false, true)
                .AddEnvironmentVariables()
                .Build();

            var minimumLogLevel = Configuration.GetValue("MinimumLogLevel", LogEventLevel.Error);

           var loggingConfigurtion = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .MinimumLevel.Information()
                .WriteTo.RollingFile(new JsonFormatter(), "logs/log-{Date}.log", minimumLogLevel, 10485760, 2);

            if(Configuration.GetValue("EnableConsoleLogging", false))
            {
                loggingConfigurtion.WriteTo.LiterateConsole(minimumLogLevel);
                loggerFactory.AddConsole(LogLevel.Error);
            }

            Log.Logger = loggingConfigurtion.CreateLogger();
        }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.TryAddSingleton(Configuration);
            services.TryAddSingleton(Log.Logger);

            services.AddOptions();
            services.AddCoreServices(Configuration);
            services.AddFileServices();

            if (HostingEnvironment.IsDevelopment())
                services.AddNullGitServices(Configuration);
            else
                services.AddGitServices(Configuration);

            services.AddSevices(Configuration);
            services.AddResponseCaching();
            services.AddMvc(options =>
            {
                options.CacheProfiles.Add("Static", new CacheProfile
                {
                    Location = ResponseCacheLocation.Any,
                    NoStore = false,
                    Duration = 360
                });

                options.CacheProfiles.Add("Content", new CacheProfile
                {
                    Location = ResponseCacheLocation.Any,
                    NoStore = false,
                    VaryByQueryKeys = new []{ "groupBy" },
                    Duration = 60
                });
                
                options.CacheProfiles.Add("None", new CacheProfile
                {
                    Location = ResponseCacheLocation.None,
                    NoStore = true
                });
                
                options.Filters.Add(new SiteVersionAttribute(Configuration));

                options.OutputFormatters.Add(new JsonOutputFormatter(new JsonSerializerSettings(), ArrayPool<char>.Shared));
                options.OutputFormatters.Add(new XmlSerializerOutputFormatter(new XmlWriterSettings { NamespaceHandling = NamespaceHandling.OmitDuplicates }));
            });

            services.AddAuthorizations(Configuration);

            return new ServiceContainer()
                .CreateServiceProvider(services);
        }

        public void Configure(IApplicationBuilder applicationBuilder)
        {
            if(HostingEnvironment.IsDevelopment())
                applicationBuilder.UseDeveloperExceptionPage();
            else
            {
                applicationBuilder.UseExceptionHandler("/error");
            }

            applicationBuilder.LoadContent();
            applicationBuilder.UseResponseCaching();
            applicationBuilder.UseStatusCodePagesWithRedirects("/error/{0}");
            applicationBuilder.UseStaticFiles();
            applicationBuilder.UseMvc();
        }
    }
}