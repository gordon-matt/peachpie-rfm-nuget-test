using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace ConsoleApp1
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            string currentDirectory = Path.GetFullPath(Path.GetRelativePath(Directory.GetCurrentDirectory(), "..\\.."));
            var root = Path.Combine(currentDirectory, "wwwroot");

            var host = new WebHostBuilder()
                .UseKestrel()
                .UseWebRoot(root)
                .UseContentRoot(root)
                .UseUrls("http://*:5004/")
                .UseStartup<Startup>()
                .ConfigureAppConfiguration((builderContext, config) =>
                {
                    config.AddJsonFile(Path.Combine(currentDirectory, "appsettings.json"), optional: false, reloadOnChange: true);
                })
                .Build();

            host.Run();
        }
    }
}