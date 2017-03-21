using System;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace CleanKludge.Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            new WebHostBuilder()
                .UseKestrel()
                .UseUrls(Environment.GetEnvironmentVariable("SERVER_URL") ?? string.Empty)
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseStartup<Startup>()
                .Build()
                .Run();
        }
    }
}