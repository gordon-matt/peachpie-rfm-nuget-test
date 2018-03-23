using System;
using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Pchp.Core;
using Peachpie.Web;

namespace ConsoleApp1
{
    internal class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            // Adds a default in-memory implementation of IDistributedCache.
            services.AddDistributedMemoryCache();
            
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = true;
            });
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseSession();

            var rfmOptions = new ResponsiveFileManagerConfig();
            Configuration.GetSection("ResponsiveFileManagerConfig").Bind(rfmOptions);
            
            app.UsePhp(new PhpRequestOptions(scriptAssemblyName: "ResponsiveFileManager")
            {
                //RootPath = Path.GetDirectoryName(Directory.GetCurrentDirectory()) + "\\Website",
                BeforeRequest = (Context ctx) =>
                {
                    // Since the config.php file is compiled, we cannot modify it once deployed... everything is hard coded there.
                    //  TODO: Place these values in appsettings.json and pass them in here to override the ones from config.php

                    ctx.Globals["appsettings"] = (PhpValue)new PhpArray()
                    {
                        { "upload_dir", rfmOptions.UploadDirectory },
                        { "current_path", rfmOptions.CurrentPath },
                        { "thumbs_base_path", rfmOptions.ThumbsBasePath }
                    };
                }
            });
            
            app.UseDefaultFiles();
            app.UseStaticFiles();
        }
    }
}