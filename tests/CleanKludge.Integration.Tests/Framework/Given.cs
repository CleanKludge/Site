using System;
using System.IO;
using System.Net.Http;
using System.Reflection;
using CleanKludge.Data.File.Articles;
using CleanKludge.Server;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.PlatformAbstractions;
using StoryTests.Prologs;

namespace CleanKludge.Integration.Tests.Framework
{
    public static class Given
    {
        private const string SolutionName = "CleanKludge";

        public static IStoryProlog<HttpClient, FakeSummaryPath, FakeArticlePath> AHttpClient()
        {
            var fakeArticlePath = new FakeArticlePath();
            var fakeSummaryPath = new FakeSummaryPath();

            var startupAssembly = typeof(Startup).GetTypeInfo().Assembly;
            var contentRoot = GetProjectPath(Path.Combine("src"), startupAssembly);

            var hostBuilder = new WebHostBuilder();
            var webHostBuilder = hostBuilder
                .UseContentRoot(contentRoot)
                .ConfigureServices(InitializeServices)
                .ConfigureServices(x => x.AddSingleton<IArticlePath>(fakeArticlePath))
                .ConfigureServices(x => x.AddSingleton<ISummaryPath>(fakeSummaryPath))
                .UseEnvironment("Development")
                .UseStartup<Startup>();

            var testHost = new TestServer(webHostBuilder);
            var httpClient = testHost.CreateClient();
            return new ThreePointProlog<HttpClient, FakeSummaryPath, FakeArticlePath>(httpClient, fakeSummaryPath, fakeArticlePath);
        }

        public static IStoryProlog<HttpClient, FakeSummaryPath, FakeArticlePath, FakeInMemoryCache> AHttpClientWithInMemoryCache()
        {
            var fakeArticlePath = new FakeArticlePath();
            var fakeSummaryPath = new FakeSummaryPath();
            var inMemoryCache = new FakeInMemoryCache();

            var hostBuilder = new WebHostBuilder();
            var webHostBuilder = hostBuilder
                .ConfigureServices(x => x.AddSingleton<IArticlePath>(fakeArticlePath))
                .ConfigureServices(x => x.AddSingleton<ISummaryPath>(fakeSummaryPath))
                .ConfigureServices(x => x.AddSingleton<IMemoryCache>(inMemoryCache))
                .UseEnvironment("Development")
                .UseStartup<Startup>();

            var testHost = new TestServer(webHostBuilder);
            var httpClient = testHost.CreateClient();
            return new FourPointProlog<HttpClient, FakeSummaryPath, FakeArticlePath, FakeInMemoryCache>(httpClient, fakeSummaryPath, fakeArticlePath, inMemoryCache);
        }

        private static void InitializeServices(IServiceCollection services)
        {
            var startupAssembly = typeof(Startup).GetTypeInfo().Assembly;

            var manager = new ApplicationPartManager();
            manager.ApplicationParts.Add(new AssemblyPart(startupAssembly));

            manager.FeatureProviders.Add(new ControllerFeatureProvider());
            manager.FeatureProviders.Add(new ViewComponentFeatureProvider());

            services.AddSingleton(manager);
        }

        private static string GetProjectPath(string solutionRelativePath, Assembly startupAssembly)
        {
            var projectName = startupAssembly.GetName().Name;
            var applicationBasePath = PlatformServices.Default.Application.ApplicationBasePath;

            var directoryInfo = new DirectoryInfo(applicationBasePath);
            do
            {
                var solutionFileInfo = new FileInfo(Path.Combine(directoryInfo.FullName, $"{SolutionName}.sln"));
                if (solutionFileInfo.Exists)
                {
                    return Path.GetFullPath(Path.Combine(directoryInfo.FullName, solutionRelativePath, projectName));
                }

                directoryInfo = directoryInfo.Parent;
            }
            while (directoryInfo.Parent != null);

            throw new Exception($"Solution root could not be located using application root {applicationBasePath}.");
        }
    }
}