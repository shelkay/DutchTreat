using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DutchTreat.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DutchTreat
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        // Dependency services
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddTransient<IMailService, NullMailService>();
            
            //--- ADD Support for real mail service

            //services.AddControllersWithViews();
            //services.AddRazorPages();
            //MvcOptions.EnableEndpointRouting = false;
            services.AddMvc(options => options.EnableEndpointRouting = false);
            //services.AddMvc();
        }
        // Middleware
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/error");
            }


            app.UseStaticFiles();
            //app.UseNodeModules();

            //app.UseRouting();
            //app.UseEndpoints(cfg =>
            //{
            //    cfg.MapControllerRoute("Fallback",
            //        "{controller}/{action}/{id?}",
            //        new { controller = "App", action = "Index" });
            //});


            app.UseMvc(cfg =>
            {
                cfg.MapRoute("Default",
                    "{controller}/{action}/{id?}",
                    new { controller = "App", action = "Index" });
            });
        }
    }
}
