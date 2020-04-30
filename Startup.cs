using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AutoMapper;
using DutchTreat.Data;
using DutchTreat.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DutchTreat
{
    public class Startup
    {
        private readonly IConfiguration _config;
        public Startup(IConfiguration config) {
            _config = config;
        }
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddDbContext<DutchContext>(cfg => {
                cfg.UseSqlServer(_config.GetConnectionString("DutchConnectionString"));
            });
            services.AddTransient<IMailService, NullMailService>();
            services.AddTransient<DutchSeeder>();

            // Create profiles of all mapping we are going to need...
            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            services.AddScoped<IDutchRepository, DutchRepository>();

            //--- ADD Support for real mail service

            //services.AddControllersWithViews();
            //services.AddRazorPages();
            //MvcOptions.EnableEndpointRouting = false;
            services.AddMvc(options => options.EnableEndpointRouting = false);
                //SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
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
