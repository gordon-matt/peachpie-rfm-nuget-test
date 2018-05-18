using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace ConsoleApp1
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var dir = Directory.GetParent(Directory.GetCurrentDirectory());
            dir = Directory.GetParent(dir.FullName);
            dir = Directory.GetParent(dir.FullName);
            string currentDirectory = dir.FullName;
            var root = Path.GetDirectoryName(currentDirectory) + "\\ConsoleApp1\\wwwroot";

            var host = new WebHostBuilder()
                .UseKestrel()
                .UseWebRoot(root)
                .UseContentRoot(root)
                .UseUrls("http://*:5004/")
                .UseStartup<Startup>()
                .ConfigureAppConfiguration((builderContext, config) =>
                {
                    config.AddJsonFile(currentDirectory + "/appsettings.json", optional: false, reloadOnChange: true);
                })
                .Build();

            host.Run();
        }
    }
}