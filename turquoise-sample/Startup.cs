using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.AspNet.Http;
using Microsoft.Extensions.DependencyInjection;
using Turquoise.Owin;
using Turquoise;

namespace turquoise_sample
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            var runtime = new Runtime();
            runtime.RegisterResource(new FooResource());
            app.UseOwin(builder => builder.UseTurquoise(runtime));
            //app.UseServer();
            
            // app.UseIISPlatformHandler();

            // app.Run(async (context) =>
            // {
            //     await context.Response.WriteAsync("Hello World!");
            // });
        }

        // Entry point for the application.
        public static void Main(string[] args) => Microsoft.AspNet.Hosting.WebApplication.Run<Startup>(args);
    }
}

/*
namespace turquoise_sample
{
    using Microsoft.AspNet.Builder;
    using Turquoise.Owin;
 
    public class Startup
    {
        public void Configure(IApplicationBuilder app)
        {
            //app.UseOwin(x => x.UseTurquoise());
            app.Use(new Runtime());
        }

        // Entry point for the application.
        public static void Main(string[] args) => Microsoft.AspNet.Hosting.WebApplication.Run<Startup>(args);
    }
}

*/